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
        public IRepository<Article> Articles { get; }

        public IRepository<Role> Roles{ get; }

        public IRepository<Client> Clients { get; }

        public IRepository<Comment> Comments { get; }

        public IRepository<Source> Sources { get; }

        public UnitOfWork(GoodAggregatorNewsContext database, IRepository<Article> articles,
            IRepository<Role> roles,IRepository<Client> client, 
            IRepository<Comment> comment, IRepository<Source> sources)
        {
            _database = database;
            Articles = articles;
            Roles = roles;
            Clients = client;
            Comments = comment;
            Sources = sources;
        }


        public async Task<int> Commit()
        {
            return await _database.SaveChangesAsync();
        }
    }
}
