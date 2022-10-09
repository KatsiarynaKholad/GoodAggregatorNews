using GoodAggregatorNews.Abstractions.Repositories;
using GoodAggregatorNews.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<Article>  Articles { get; }
        IRepository<Role> Roles { get; }
        IRepository<Client> Clients { get; }
        IRepository<Comment> Comments { get; }
        IRepository<Source> Sources { get; }
        Task<int> Commit();
    }
}
