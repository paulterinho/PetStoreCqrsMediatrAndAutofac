using Petstore.Swagger.Io.Common.Utils;
using PetStore.Domain.Infrastructure.Common;
using PetStore.Domain.Infrastructure.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Infrastructure.Models
{
    [Table("Pet", Schema = "petCommand")]
    public partial class Pet :
        IDbEntity,
        ICreateModifyDeleteTimesUTC, // makes us use dates for updating / removing / deleting
        IUpdatable<Pet> // Allows us to use this in a generic repository

    {
        [Key]
        public int ID { get; set; }

        public new Guid ResourceID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }


        [Column(TypeName = "datetime2")]
        public DateTime CreatedDateTimeUTC { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ModifiedDateTimeUTC { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RemovedDateTimeUTC { get; set; }


        #region helpers

        /// <summary>
        /// Update an existing record state TO that of another's
        /// </summary>
        /// <param name="updatedPet">The pet whose values have been changed.</param>
        public void Update(Pet updatedPet)
        {
            try
            {
                // omit ResourceID, CreatedDateTimeUTC

                this.ModifiedDateTimeUTC = updatedPet.ModifiedDateTimeUTC;
                this.Name = updatedPet.Name;
                this.RemovedDateTimeUTC = updatedPet.RemovedDateTimeUTC;

            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }

        #endregion

        #region Helpers

        private void RemoveOldItemsAndAddNewItems<T>(ref ICollection<T> collection, ICollection<T> updatedCollection)
            where T : ICreateModifyDeleteTimesUTC
        {
            try
            {

                if (collection != null)
                {
                    // we don't care about the history of items (when they were removed) so just remove all of the old ones, and add the new ones
                    foreach (var removableItem in collection)
                    {
                        removableItem.RemovedDateTimeUTC = DateTime.UtcNow;
                        removableItem.ModifiedDateTimeUTC = DateTime.UtcNow;
                    }

                    if (updatedCollection != null)
                    {
                        // Apparently ICollection doesn't have AddRange. Do it the old-fashsioned way. 
                        foreach (T newItem in updatedCollection)
                        {
                            newItem.CreatedDateTimeUTC = DateTime.UtcNow;
                            newItem.ModifiedDateTimeUTC = DateTime.UtcNow;
                            collection.Add(newItem);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }

        #endregion
    }
}