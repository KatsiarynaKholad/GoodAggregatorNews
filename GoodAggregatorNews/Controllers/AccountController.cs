using AutoMapper;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace GoodAggregatorNews.Controllers
{
    public class AccountController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public AccountController(IClientService clientService,
            IRoleService roleService,
            IMapper mapper)
        {
            _clientService = clientService;
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var clientRoleId = await _roleService.GetRoleIdByNameAsync("User");

                    var clientDto = _mapper.Map<ClientDto>(model);
                    if (clientDto != null && clientRoleId != null)
                    {
                        clientDto.RoleId = clientRoleId.Value;
                        var result = await _clientService.RegisterUser(clientDto, model.Password);
                        if (result > 0)
                        {
                            await Authenticate(model.Email);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Register was not successful");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckEmail(string email)
        {
            try
            {
                var clientdDto = await _clientService.GetUserByEmailAsync(email);

                if (clientdDto != null)
                {
                    return Ok(false);
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: CheckEmail was not successful");
                throw;
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isPasswordCorrect = await _clientService.CheckUserPassword(model.Email, model.Password);
                    if (isPasswordCorrect)
                    {
                        await Authenticate(model.Email);

                        return RedirectToAction("Index", "Article");

                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Login was not successful");
                throw;
            }
        }

        private async Task Authenticate(string email)
        {
            try
            {
                var userDto = await _clientService.GetUserByEmailAsync(email);

                var claims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userDto.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userDto.RoleName)
                };

                var identity = new ClaimsIdentity(claims,
                    "ApplicationCookie",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType
                );
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Authenticate was not successful");

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Logout was not successful");

                throw;
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClientData()
        {
            var clientEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(clientEmail))
            {
                return BadRequest();
            }

            var client = _mapper.Map<ClientDataModel>(await _clientService.GetUserByEmailAsync(clientEmail));
            return Ok(client);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult IsLoggedIn()
        {
            if (User.Identities.Any(identity => identity.IsAuthenticated))
            {
                return Ok(true);
            }

            return Ok(false);
        }

        [HttpGet]
        public async Task<IActionResult> ClientLoginPreview()
        {
            if (User.Identities.Any(identity => identity.IsAuthenticated))
            {
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest();
                }

                var user = _mapper.Map<ClientDataModel>(await _clientService.GetUserByEmailAsync(userEmail));
                return View(user);
            }

            return View();
        }

    }
}
