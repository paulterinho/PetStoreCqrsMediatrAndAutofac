using Dapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Petstore.Common.Command;
using Petstore.Common.Utils;
using PetStore.Common.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PetStore.API.Application.Query.DB
{
    /// <summary>
    /// This class's life cycle is being managed by Autofac. Check out PetStoreApplicationModule for more context. 
    /// </summary>
    public class PetStoreQueriesRepository : IPetStoreQueriesRepository
    {
        private readonly string connectionString = string.Empty;
        private readonly ILogger _logger;

        public Dictionary<string, PetTypeValue> PetTypeDictionary { get; }

        /// <summary>
        /// This is pushed in from Autofac. Check out the PetStoreApplicatoinModule.cs for more context.
        /// </summary>
        /// <param name="queriesConnectionString"></param>
        public PetStoreQueriesRepository(ILogger logger, ISecretsManager petSecretsManager)
        {
            try
            {
                connectionString = petSecretsManager.GetDbConnectionString();
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));

                // Create a cached Dictionary of PetStoreAuthority Enums based on their Value property for massive Big-O time savings.
                PetTypeDictionary = EnumUtils.CreateDictionaryByValue<PetTypeValue>();
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
        }

        /// <summary>
        /// Get a specific pet by it's ResourceID
        /// </summary>
        public async Task<Pet> ShowPetById([BindRequired] string resourceID, CancellationToken cancellationToken = default)
        {
            Pet firstPet = null;
            try
            {
                string sql = @" SELECT CAST(ResourceID AS VARCHAR(100)) ResourceID, 
                                    Name, Type
                                FROM [petQuery].[Pet] 
                                WHERE ResourceID = @PetStoreId";


                // Note: no need to split...yet. (APIs always grow). I know..YAGNI XD
                string SPLIT_ON = @"";

                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    object dapperParams = new { PetStoreId = resourceID };

                    IDbTransaction transaction = null;
                    bool buffered = true;
                    int? commandTimeout = null;
                    CommandType commandType = CommandType.Text;

                    // Get the list bc it allows us to pass a mapping object. (Why does dapper now have this for their methods that return a single object?)
                    var pets = await db.QueryAsync<Pet>(sql,

                            // If you are sending more than 7 types, Dapper needs you to send an array of types.
                            //  @see https://riptutorial.com/dapper/example/1198/mapping-more-than-7-types
                            new[]
                            {
                                typeof(Pet),          // 0
                            },

                            // Inner method we are passing to Dapper to marshal pets correctly
                            (results) =>
                            {
                                Pet pet = (Pet)results[0];

                                return pet;
                            },
                            dapperParams, transaction, buffered, SPLIT_ON, commandTimeout, commandType);

                    firstPet = pets.FirstOrDefault();
                }

            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            // Convert to API object
            return firstPet;
        }

        /// <summary>
        /// Gets a list of PetStore Objects potentially sorted / filtered by the params passed to it.
        /// </summary>
        public async Task<PetCollection> ListPets(
            int? limit,
            int? offset,
            IEnumerable<PetSortValue> sorts = null,
            IEnumerable<string> namesToFilterBy = null,
            IEnumerable<PetTypeValue> typesToFilterBy = null,
            CancellationToken cancellationToken = default)
        {
            // NOTE:    Nice and sneaky: This is the name of the derived column that allows
            //          us to get the total of a paged result set without running the query twice.
            //
            //  @see https://stackoverflow.com/questions/19125347/get-total-row-count-while-paging
            //
            const string TotalCount = "TotalCount";
            const string TotalCountClause = TotalCount + @" = COUNT(*) OVER()";

            PetCollection petCollection;
            IEnumerable<Pet> pets;
            string SPLIT_ON = "" + TotalCount; // NOTE: Keep the Total Counts split at the end. If you are going to add additional splits, add them before this.
            string orderClause = "";
            string limitAndOffsetClause = "";
            object dapperParams;
            int total = -1;

            string SQL = @" SELECT CAST(ResourceID AS VARCHAR(100)) ResourceID, 
                                Name, Type,
                                " + TotalCountClause + @"
                            FROM [petQuery].[Pet]
                         ";
            // incoming enums aren't using their display value so they need to be converted before using in our dapper query.
            IEnumerable<string> convertedTypeEnumsToFilterBy = null;

            string derivedSQL = SQL;

            try
            {
                ///
                /// INNER METHOD: Inner method to create the SQL needed to get a list of pets
                /// 
                string buildSql()
                {
                    SqlBuilder builder = new SqlBuilder();
                    string returnSql = "";

                    //note the 'where' in-line comment is required, it is a replacement token
                    var selector = builder.AddTemplate(SQL + " /**where**/ /**orderby**/ ");

                    if ((namesToFilterBy != null) && (namesToFilterBy.Count() > 0))
                    {
                        builder.Where("Name IN @Names");
                    }

                    if ((typesToFilterBy != null) && (typesToFilterBy.Count() > 0))
                    {
                        builder.Where("Type IN @Types");

                        // TODO: figure out how to make a dictionary of enums, with the enum as the key and the human readable string as the value. 
                        convertedTypeEnumsToFilterBy = typesToFilterBy.Select(ws => EnumUtils.GetEnumMemberAttrValue<PetTypeValue>(ws))
                            .ToList();
                    }

                    orderClause = SqlUtils.CreateOrderByClause<PetSortValue>(sorts, PetSortValue.Name_ASC);
                    limitAndOffsetClause = SqlUtils.GetLimitAndOffSetClause(limit, offset, orderClause);

                    returnSql += selector.RawSql + " " + orderClause + " " + limitAndOffsetClause;

                    return returnSql;
                }

                ///
                /// INNER METHOD: Inner method to create a list of PetStore Objects
                /// 
                async Task<IEnumerable<Pet>> marshalPetStore(string sql)
                {
                    IEnumerable<Pet> _pets;
                    // Authorities needs special handling to convert to the right enum.

                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        IDbTransaction transaction = null;
                        bool buffered = true;
                        int? commandTimeout = null;
                        CommandType commandType = CommandType.Text;

                        dapperParams = new
                        {
                            Limit = limit,
                            Names = namesToFilterBy,
                            Types = convertedTypeEnumsToFilterBy
                        };

                        // Marshal the pets
                        _pets = await db.QueryAsync<Pet>(sql,

                            // If you are sending more than 7 types, Dapper needs you to send an array of types.
                            //  @see https://riptutorial.com/dapper/example/1198/mapping-more-than-7-types
                            new[]
                            {
                                typeof(Pet),
                                //typeof(string), // Type, because it's an Enum we'll need to handle it separately.
                                typeof(System.Int32)  // TotalCount
                            },

                            // Inner method we are passing to Dapper to marshal pets correctly
                            (results) =>
                            {
                                Pet pet = (Pet)results[0];

                                // NOTE:    See this assignment with `total`, it's a variable external to this method,
                                //          and it's accessible because we are using a closure 💪
                                total = (int)results[1];

                                return pet;
                            },
                            dapperParams, transaction, buffered, SPLIT_ON, commandTimeout, commandType);

                    } // end `using`

                    return _pets;
                }

                derivedSQL = buildSql();
                pets = await marshalPetStore(derivedSQL);

                // package the pets
                petCollection = new PetCollection()
                {
                    Offset = offset,
                    PageSize = limit,
                    Total = total, // add the totals, which may be different than PetStore.Count()
                    Pets = (List<Pet>)pets
                };

            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            // Convert to API object
            return petCollection;
        }

        public async Task<IEnumerable<string>> GetPetStoreNames(CancellationToken cancellationToken)
        {
            IEnumerable<string> petNames = null;

            const string sql = @" SELECT Name
                                FROM [petQuery].[Pet]
                                ORDER BY Name ASC";
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    petNames = await db.QueryAsync<string>(sql);
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            // Convert to API object
            return petNames;
        }

    }
}