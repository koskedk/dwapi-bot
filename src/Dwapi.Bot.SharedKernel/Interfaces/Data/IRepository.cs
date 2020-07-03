using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.SharedKernel.Interfaces.Data
{
    public interface IRepository<T, in TId>  where T : Entity<TId>
    {
        Task<TC> GetByIdAsync<TC, TCId>(TCId id) where TC : Entity<TCId>;
        Task<int> GetCount<TC, TCId>() where TC : Entity<TCId>;
        Task<int> GetCount<TC, TCId>(Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>;
        IQueryable<TC> GetAll<TC, TCId>() where TC : Entity<TCId>;
        IQueryable<TC> GetAll<TC, TCId>(Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>;
        IQueryable<TC> GetAllPaged<TC, TCId>(int page, int pageSize,string orderBy) where TC : Entity<TCId>;
        IQueryable<TC> GetAllPaged<TC, TCId>(int page, int pageSize,string orderBy,Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>;
        Task<bool> ExistsAsync<TC, TCId>(TC entity) where TC : Entity<TCId>;
        Task<bool> ExistsAsync<TC, TCId>(Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>;
        Task CreateOrUpdateAsync<TC, TCId>(IEnumerable<TC> entities) where TC : Entity<TCId>;
        Task Merge<TC, TCId>(IEnumerable<TC> entities) where TC : Entity<TCId>;
        Task ExecCommand(string sqlCommand);
        Task SaveAsync();
        IDbConnection GetConnection(bool open = true);
        IDbConnection GetConnectionOnly();
    }
}
