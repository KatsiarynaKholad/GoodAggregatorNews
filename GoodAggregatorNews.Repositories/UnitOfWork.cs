using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Abstractions.Repositories;
using GoodAggregatorNews.Database;
using GoodAggregatorNews.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GoodAggregatorNewsContext _database;
        public IAdditionArticleRepository Articles { get; }

        public IRepository<Role> Roles { get; }

        public IRepository<Client> Clients { get; }

        public IRepository<Comment> Comments { get; }

        public IRepository<Source> Sources { get; }

        public UnitOfWork(GoodAggregatorNewsContext database, IAdditionArticleRepository articles,
            IRepository<Role> roles, IRepository<Client> clients,
            IRepository<Comment> comments, IRepository<Source> sources)
        {
            _database = database;
            Articles = articles;
            Roles = roles;
            Clients = clients;
            Comments = comments;
            Sources = sources;
        }


    public async Task<int> Commit()
        {
            return await _database.SaveChangesAsync();
        }
    }
}
