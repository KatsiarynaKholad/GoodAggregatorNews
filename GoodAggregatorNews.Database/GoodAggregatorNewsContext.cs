using GoodAggregatorNews.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Database
{
    public class GoodAggregatorNewsContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Source> Sources { get; set; }

        public GoodAggregatorNewsContext(DbContextOptions<GoodAggregatorNewsContext> options)
            : base(options)
        {
        }
    }
}
