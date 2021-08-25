using Microsoft.EntityFrameworkCore;
using PetStore.Domain.Infrastructure.Common;

namespace ServiceSeedwork.Utils
{
    /// <summary>
    /// Used as a callback on an abstract class to ensure we can generically get the main (Aggregate Root) DB Set so we can do operations on it (Add, Remove, Update).
    /// </summary>
    /// <typeparam name="InfrastructureModel"></typeparam>
    interface IGetMainDbSet<InfrastructureModel>
        where InfrastructureModel : IDbEntity
    {
        DbSet<InfrastructureModel> GetMainDbSet();
    }
}
