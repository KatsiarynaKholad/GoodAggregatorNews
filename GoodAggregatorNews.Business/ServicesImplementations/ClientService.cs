﻿using AutoMapper;
using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Business.ServicesImplementations
{
    public class ClientService : IClientService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckUserPassword(string email, string password)
        {
            try
            {
                var dbPasswordHash = (await _unitOfWork.Clients.Get()
                         .FirstOrDefaultAsync(client => client.Email.Equals(email)))
                         ?.PasswordHash;

                return
                     dbPasswordHash != null
                     && CreateMd5(password).Equals(dbPasswordHash);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: CheckUserPassword was not successful");
                throw;
            }
          
        }

        public async Task<bool> CheckUserPassword(Guid userId, string password)
        {
            try
            {
                var dbPasswordHash = (await _unitOfWork.Clients.GetByIdAsync(userId))?.PasswordHash;

                return
                    dbPasswordHash != null
                    && CreateMd5(password).Equals(dbPasswordHash);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: CheckUserPassword was not successful");
                throw;
            }
        }

        public async Task<ClientDto?> GetUserByEmailAsync(string email)
        {
            try
            {
                var client = await _unitOfWork.Clients
                 .FindBy(cl => cl.Email.Equals(email),
                     cl => cl.Role)
                 .FirstOrDefaultAsync();

                if (client != null)
                {
                    var dto = _mapper.Map<ClientDto>(client);
                    return dto;
                }

                return null;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetUserByEmailAsync was not successful");
                throw;
            }
        }

        public async Task<IEnumerable<ClientDto>> GetAllUsersAsync()
        {
            try
            {
                var res = (await _unitOfWork.Clients.Get().ToListAsync())
                    .Select(ent=>_mapper.Map<ClientDto>(ent))
                    .ToList();

                if (res!=null)
                {
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: IsUserExists was not successful");
                throw;
            }
        
        }

        public async Task<bool> IsUserExists(Guid clintId)
        {
            try
            {
                if (!Guid.Empty.Equals(clintId))
                {
                    return await _unitOfWork.Clients.Get().AnyAsync(client => client.Id.Equals(clintId));
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: IsUserExists was not successful");
                throw;
            }
        }

        public async Task<bool> IsUserExists(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                { 
                    return await _unitOfWork.Clients.Get().AnyAsync(client => client.Email.Equals(email));
                }
                return false;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: IsUserExists was not successful");
                throw;
            }
        }

        public async Task DeleteClientAsync(Guid id)
        {
            try
            {
                if (!Guid.Empty.Equals(id))
                {
                    var ent = await _unitOfWork.Clients.GetByIdAsync(id);
                    if (ent!=null)
                    {
                        _unitOfWork.Clients.Remove(ent);
                        await _unitOfWork.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: DeleteClientAsync was not successful");
                throw;
            }
        
        }

        public async Task<int> RegisterUser(ClientDto dto, string password)
        {
            try
            {
                var entity = _mapper.Map<Client>(dto);

                entity.PasswordHash = CreateMd5($"{password}.{_configuration["ClientSecrets:PasswordSalt"]}");

                await _unitOfWork.Clients.AddAsync(entity);
                return await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: RegisterUser was not successful");
                throw;
            }
        }

        private string CreateMd5(string password)
        {
            try
            {
                var passwordSalt = _configuration["ClientSecrets:PasswordSalt"];

                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    var inputBytes = System.Text.Encoding.UTF8.GetBytes(password + passwordSalt);
                    var hashBytes = md5.ComputeHash(inputBytes);

                    return Convert.ToHexString(hashBytes);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: CreateMd5 was not successful");
                throw;
            }
        }
    }
}
