using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Database.Entities
{
    public class Role : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Client> Clients { get; set; }
    }
}
