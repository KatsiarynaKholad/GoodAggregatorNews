using AutoMapper;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.WebAPI.Models.Requests;
using GoodAggregatorNews.WebAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoodAggregatorNews.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with RefreshToken
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly IJwtUtil _jwtUtil;

        public RefreshTokenController(IClientService clientService,
            IMapper mapper,
            IJwtUtil jwtUtil)
        {
            _clientService = clientService;
            _mapper = mapper;
            _jwtUtil = jwtUtil;
        }

        /// <summary>
        ///  RefreshToken
        /// </summary>
        /// <param name = "requestModel" ></ param >
        /// < returns ></ returns >
        [Route("Refresh")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel requestModel)
        {
            try
            {
                var client = await _clientService.GetClientByRefreshTokenAsync(requestModel.RefreshToken);
                if (client != null)
                {
                    var response = await _jwtUtil.GenerateTokenAsync(client);
                    await _jwtUtil.RemoveRefreshTokenAsync(requestModel.RefreshToken);
                    return Ok(response);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: RefreshToken was not successful");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Remove refreshToken
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [Route("Revoke")]
        [HttpPost]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestModel requestModel)
        {
            try
            {
                await _jwtUtil.RemoveRefreshTokenAsync(requestModel.RefreshToken);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: RevokeToken was not successful");
                return StatusCode(500);
            }
        }
    }
}
