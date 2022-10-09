using GoodAggregatorNews.Abstractions.Repositories;
using GoodAggregatorNews.Core;
using GoodAggregatorNews.Database;
using GoodAggregatorNews.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly GoodAggregatorNewsContext Database;
        protected readonly DbSet<T> DbSet;

        public Repository(GoodAggregatorNewsContext database)
        {
            Database = database;
            DbSet = database.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Add entity was not successful");
                throw;
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                await DbSet.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AddRange entities was not successful");
                throw;
            }
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T,
            bool>> searchExpression, params Expression<Func<T, 
                object>>[] includes)
        {
            try
            {
                var result = DbSet.Where(searchExpression);
                if (includes.Any())
                {
                    result = includes.Aggregate(result, (current, include)
                        => current.Include(include));
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: FindBy was not successful");
                throw;
            }
        }

        public virtual IQueryable<T> Get()
        {
            try
            {
                return DbSet.AsQueryable();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Get was not successful");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await DbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetAll was not successful");
                throw;
            }
        }

        public virtual async Task<T> GetByIdAsync(Guid Id)
        {
            try
            {
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(entity=>entity.Id.Equals(Id));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetById was not successful");
                throw;
            }
        }

        public virtual async Task PatchAsync(Guid Id, List<PatchModel> patchData)
        {
            try
            {
                var model = DbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(Id));

                var nameValuePropertiesPairs = patchData.ToDictionary(
                    patchmodel => patchmodel.PropertyName,
                    patchmodel => patchmodel.PropertyValue);

                var dbEntityEntry = Database.Entry(model);

                dbEntityEntry.CurrentValues.SetValues(nameValuePropertiesPairs);
                dbEntityEntry.State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Patch was not successful");
                throw;
            }
        }

        public virtual void Remove(T entity)
        {
            try
            {
                DbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Remove was not successful");
                throw;
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                DbSet.UpdateRange(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Update was not successful");
                throw;
            }
        }
    }
}
