using AutoMapper;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.WebAPI.Models.Requests;
using GoodAggregatorNews.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoodAggregatorNews.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with client
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IJwtUtil _jwtUtil;
        public ClientsController(IClientService clientService, 
            IRoleService roleService,
            IMapper mapper,
            IJwtUtil jwtUtil)
        {
            _clientService = clientService;
            _roleService = roleService;
            _mapper = mapper;
            _jwtUtil = jwtUtil;
        }

        /// <summary>
        /// Get all clients (only Role:Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllClient()
        {
            try
            {
                var clients = await _clientService.GetAllUsersAsync();
                if (clients!=null)
                {
                    return Ok(clients);

                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetAllClient in ClientsController is not successful");
                throw;
            }
        }

        /// <summary>
        /// Delete client (only Role: Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            try
            {
                if (!Guid.Empty.Equals(id))
                {
                    await _clientService.DeleteClientAsync(id);
                    return Ok();

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: DeleteClient in ClientsController is not successful");
                throw;
            }
        }

        /// <summary>
        /// Register client
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] RegisterClientRequestModel model)
        {
            try
            {
                var clientRoleId = await _roleService.GetRoleIdByNameAsync("User");
                var dto = _mapper.Map<ClientDto>(model);
                var isClientExist = await _clientService.IsUserExists(model.Email);
                if (clientRoleId!=null
                    && dto!=null
                    && !isClientExist
                    && model.Password.Equals(model.ConfirmationPassword))
                {
                    dto.RoleId = clientRoleId.Value;
                    var res = await _clientService.RegisterUser(dto, model.Password);
                    if (res>0)
                    {
                        var clientInDb = await _clientService.GetClientByEmailAsync(dto.Email);
                        var response = _jwtUtil.GenerateToken(clientInDb);
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest();

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: CreateClient in ClientsController is not successful");
                throw;
            }

        }


    }
}
