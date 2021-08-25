using System;

namespace PetStore.Domain.Infrastructure.Common
{
    /// <summary>
    /// Marker Interface for Infrastructure Entities that can be persisted to a relational DB
    /// </summary>
    public class IDbEntity
    {
        public Guid ResourceID { get; set; }
    }
}