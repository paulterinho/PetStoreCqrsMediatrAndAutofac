using System;

namespace PetStore.Domain.Infrastructure.Models
{
    /// <summary>
    /// Interface that allows us to use generics when needing to do the boiler plate activities of deleting old items, and adding new ones in the DB.
    /// </summary>
    public interface ICreateModifyDeleteTimesUTC
    {
        //[Column(TypeName = "datetime2")]
        DateTime CreatedDateTimeUTC { get; set; }

        //[Column(TypeName = "datetime2")]
        DateTime ModifiedDateTimeUTC { get; set; }

        //[Column(TypeName = "datetime2")]
        DateTime? RemovedDateTimeUTC { get; set; }
    }
}