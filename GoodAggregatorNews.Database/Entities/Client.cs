using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Database.Entities
{
    public class Client : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistationDate { get; set; }
        public List<Comment> Comments { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
