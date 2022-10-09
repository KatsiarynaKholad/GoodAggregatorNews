using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Business.ServicesImplementations
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid?> GetRoleIdByNameAsync(string name)
        {
            try
            {
                var role = await _unitOfWork.Roles.FindBy(role => role.Name.Equals(name))
                    .FirstOrDefaultAsync();
                if (role!=null)
                {
                    return role.Id;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetRoleIdByNameAsync was not successful");
                throw;
            }
        }

        public async Task<string> GetRoleNameById(Guid Id)
        {
            try
            {
                var role = await _unitOfWork.Roles.GetByIdAsync(Id);
                if (role!=null)
                {
                    return role.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetRoleNameById was not successful");
                throw;
            }
        }
    }
}
