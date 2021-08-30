using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Petstore.Common.Utils
{
    public class SqlUtils
    {

        /**
         * For Offset/Fetch to work in SQL Server, you'll need to include an order by clause. If one doesn't exist, use:
         *      
         *      ORDER BY (SELECT NULL)
         *      OFFSET 1 ROWS
         *      FETCH NEXT 2 ROWS ONLY;
         *  
         *  @see https://stackoverflow.com/questions/109232/what-is-the-best-way-to-paginate-results-in-sql-server
         */
        public static string GetLimitAndOffSetClause(int? limit, int? offset, string orderByClause)
        {
            const string ORDER_BY_DEFAULT = " ORDER BY (SELECT NULL) ";
            const string FETCH_NEXT = " FETCH NEXT ";
            const string OFFSET = " OFFSET ";
            const string ROWS = " ROWS ";
            const string ROWS_ONLY = " ROWS ONLY ";
            string clause = "";

            try
            {
                // add a default order by clause so Fetch/Offset if none exists.
                if ((orderByClause == null) && (orderByClause == ""))
                {
                    clause += ORDER_BY_DEFAULT;
                }

                // sql server has zero based indexing for rows.
                if ((offset != null) && (offset > 0))
                {
                    // SQL Server's concept of offset is different than every other DBs. Derive the value we need. 
                    var rowOffset = (offset * limit) - limit;
                    clause += OFFSET + rowOffset + ROWS;
                }

                if ((limit != null) && (limit > 0))
                {
                    clause += FETCH_NEXT + limit + ROWS_ONLY;
                }
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            return clause;
        }

        /// <summary>
        /// Create the ORDER BY clase for the SQL.
        ///  
        /// It avoids SQL injection bc we are using Enums.
        /// </summary>
        public static string CreateOrderByClause<T>(IEnumerable<T> _sort, T defaultSort)
            where T : Enum
        {
            string orderByClause = "ORDER BY ";
            string ordersSqlStr = "";

            // The constants are just SQL when you remove the underscore. This can be more performant. Change if when you do.
            static string GetSQL(T petSort)
            {
                string sql = EnumUtils.GetEnumMemberAttrValue<T>(petSort);
                sql = sql.Replace("_ASC", " ASC");
                sql = sql.Replace("_DESC", " DESC");
                return sql;
            }

            try
            {

                // handle sorts, see if we need to append to the base SQL.
                if (_sort.Count() < 1)
                {
                    // add a default sort
                    ordersSqlStr = GetSQL(defaultSort);//T.Name_ASC);
                }
                else
                {
                    // append the the Order By clauses defined in the "sort" collection.
                    List<string> allTheSortClauses = new List<string>();

                    // use old fashioned loop bc linq will ignore exceptions thrown.
                    foreach (T ws in _sort)
                    {
                        allTheSortClauses.Add(GetSQL(ws));
                    }

                    var sqlStr = String.Join(", ", allTheSortClauses);

                    ordersSqlStr = sqlStr;
                }

                orderByClause += ordersSqlStr;

            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return orderByClause;
        }
    }
}