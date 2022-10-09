using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.Abstractions
{
    public interface IRoleService
    {
        Task<string> GetRoleNameById(Guid Id);
        Task<Guid?> GetRoleIdByNameAsync(string name);
    }
}
