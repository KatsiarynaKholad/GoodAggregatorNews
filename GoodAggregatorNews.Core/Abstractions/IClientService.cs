using GoodAggregatorNews.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.Abstractions
{
    public interface IClientService
    {
        Task<bool> IsClientExists(Guid userId);
        Task<bool> CheckClientPassword(string email, string password);
        Task<bool> CheckClientPassword(Guid userId, string password);
        Task<int> RegisterClient(ClientDto dto, string password);
        Task<ClientDto?> GetClientByEmailAsync(string email);
        Task<IEnumerable<ClientDto>> GetAllUsersAsync();
        Task DeleteClient(Guid id);
        Task<bool> IsClientExists(string email);
        Task<ClientDto?> GetClientByRefreshTokenAsync(Guid token);
    }
}
