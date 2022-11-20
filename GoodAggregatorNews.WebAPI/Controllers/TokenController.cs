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
    /// Controller for work with Token
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IClientService _clietnService;
        private readonly IMapper _mapper;
        private readonly IJwtUtil _jwtUtil;

        public TokenController(IClientService clietnService,
            IMapper mapper,
            IJwtUtil jwtUtil)
        {
            _clietnService = clietnService;
            _mapper = mapper;
            _jwtUtil = jwtUtil;
        }

        /// <summary>
        /// Create JwtToken
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginClientRequestModel requestModel)
        {
            try
            {
                var client = await _clietnService.GetUserByEmailAsync(requestModel.Email);
                if (client != null)
                {
                    var isPasswordCorrect = await _clietnService.CheckUserPassword(requestModel.Email, 
                        requestModel.Password);
                    if (isPasswordCorrect)
                    {
                        var response = _jwtUtil.GenerateToken(client);
                        return Ok(response);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Authenticate was not successful");
                throw;
            }
        }
    }
}
