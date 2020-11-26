using System;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Catalogs
{
    public interface ISiteRepository : IRepository<Site, Guid>
    {
    }
}
