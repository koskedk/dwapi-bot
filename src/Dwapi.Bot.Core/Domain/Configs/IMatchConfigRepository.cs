using System;
using System.Collections.Generic;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Configs
{
    public interface IMatchConfigRepository:IRepository<MatchConfig,Guid>
    {
        IEnumerable<MatchConfig> GetConfigs();
    }
}
