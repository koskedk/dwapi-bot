using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Interfaces.Data;
using Dwapi.Bot.SharedKernel.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
   public abstract class BaseRepository<T, TId> : IRepository<T, TId> where T : Entity<TId>
    {
        protected internal DbContext Context;
        protected internal readonly DbSet<T> DbSet;
        private IDbConnection _connection;

        public string ConnectionString => Context.Database.GetDbConnection().ConnectionString;

        protected BaseRepository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }
        public virtual Task<TC> GetByIdAsync<TC, TCId>(TCId id) where TC : Entity<TCId>
        {
            return GetAll<TC, TCId>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<int> GetCount<TC, TCId>() where TC : Entity<TCId>
        {
            var count = await GetAll<TC, TCId>()
                .Select(x => x.Id)
                .CountAsync();
            return count;
        }

        public async Task<int> GetCount<TC, TCId>(Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>
        {
            var count = await GetAll<TC, TCId>()
                .Where(predicate)
                .Select(x => x.Id)
                .CountAsync();
            return count;;
        }

        public virtual IQueryable<TC> GetAll<TC, TCId>() where TC : Entity<TCId>
        {
            return Context.Set<TC>().AsNoTracking();
        }

       public virtual IQueryable<TC> GetAll<TC, TCId>(Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>
        {
            return GetAll<TC, TCId>().Where(predicate);
        }

       public IQueryable<TC> GetAllPaged<TC, TCId>(int page, int pageSize,string orderBy) where TC : Entity<TCId>
       {
           page = page < 0 ? 1 : page;
           pageSize = pageSize < 0 ? 1 : pageSize;

           var query = GetAll<TC, TCId>();

           return query
               .OrderBy(orderBy)
               .Skip((page - 1) * pageSize)
               .Take(pageSize);
       }

       public IQueryable<TC> GetAllPaged<TC, TCId>(int page, int pageSize, string orderBy, Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>
       {
           page = page < 0 ? 1 : page;
           pageSize = pageSize < 0 ? 1 : pageSize;

           var query = GetAll<TC, TCId>().Where(predicate);

           return query
               .OrderBy(orderBy)
               .Skip((page - 1) * pageSize)
               .Take(pageSize);
       }

       public virtual async Task<bool> ExistsAsync<TC, TCId>(TC entity) where TC : Entity<TCId>
       {
           return null != await GetByIdAsync<TC, TCId>(entity.Id);
       }
        public virtual async Task<bool> ExistsAsync<TC, TCId>(Expression<Func<TC, bool>> predicate)
            where TC : Entity<TCId>
        {
            var entity = await GetAll<TC, TCId>().FirstOrDefaultAsync(predicate);
            return null != entity;
        }

        public virtual async Task CreateOrUpdateAsync<TC, TCId>(IEnumerable<TC> entities) where TC : Entity<TCId>
        {
            var updates = new List<TC>();
            var inserts = new List<TC>();

            foreach (var entity in entities)
            {
                var exists = await ExistsAsync<TC,TCId>(entity);
                if (exists)
                    updates.Add(entity);
                else
                    inserts.Add(entity);
            }

            if (inserts.Any())
                GetConnection().BulkInsert(inserts);

            if (updates.Any())
                GetConnection().BulkUpdate(updates);
        }

        public async Task Merge<TC, TCId>(IEnumerable<TC> entities) where TC : Entity<TCId>
        {
            using (var cn = GetConnectionOnly())
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();
                await cn.BulkActionAsync(x => x.BulkMerge(entities));
            }
        }

        public virtual IDbConnection GetConnection(bool open = true)
        {
            if (null == _connection)
            {
                if (Context.Database.IsSqlServer())
                    _connection = new SqlConnection(ConnectionString);

                if (Context.Database.IsSqlite())
                    _connection = new SqliteConnection(ConnectionString);
            }

            if (null == _connection)
                throw new Exception("Database provider error");

            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            return _connection;
        }

        public IDbConnection GetConnectionOnly()
        {
            IDbConnection cn = null;
            if (Context.Database.IsSqlServer())
                cn = new SqlConnection(ConnectionString);

            if (Context.Database.IsSqlite())
                cn = new SqliteConnection(ConnectionString);

            return cn;
        }

        public Task ExecCommand(string sqlCommand)
        {
            return Context.Database.ExecuteSqlRawAsync(sqlCommand);
        }

        public virtual Task SaveAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
