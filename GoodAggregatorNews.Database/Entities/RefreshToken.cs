using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Database.Entities
{
    public class RefreshToken : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid Token { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
    }
}
