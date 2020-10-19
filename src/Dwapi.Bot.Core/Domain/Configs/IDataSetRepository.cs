using System;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Configs
{
    public interface IDataSetRepository:IRepository<DataSet,Guid>
    {
        DataSet GetByName(string name);
    }
}