using GoodAggregatorNews.Core;
using GoodAggregatorNews.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Abstractions.Repositories
{
    public interface IRepository<T> where T : IBaseEntity
    {
        Task<T> GetByIdAsync(Guid Id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Get();
        IQueryable<T> FindBy(Expression<Func<T, bool>> searchExpression,
            params Expression<Func<T, object>>[] includes);

        Task AddAsync (T entity);
        Task AddRangeAsync (IEnumerable<T> entities);

        void Update (T entities);
        Task PatchAsync(Guid Id, List<PatchModel> patchData);

        void Remove(T entity);

    }
}
