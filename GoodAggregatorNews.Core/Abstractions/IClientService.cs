﻿using GoodAggregatorNews.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.Abstractions
{
    public interface IClientService
    {
        Task<bool> IsUserExists(Guid userId);
        Task<bool> CheckUserPassword(string email, string password);
        Task<bool> CheckUserPassword(Guid userId, string password);
        Task<int> RegisterUser(ClientDto dto, string password);
        Task<ClientDto?> GetClientByEmailAsync(string email);
        Task<IEnumerable<ClientDto>> GetAllUsersAsync();
        Task DeleteClientAsync(Guid id);
        Task<bool> IsUserExists(string email);
    }
}
