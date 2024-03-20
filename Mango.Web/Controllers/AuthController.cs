using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Services.IService;
using Mango.Web.Ultility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var responseDto = await _authService.LoginAsync(loginRequestDto);
            if (responseDto != null && responseDto.IsSuccess)
            {
                var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Data));

                _tokenProvider.SetToken(loginResponseDto.Token);
                await SignInUser(loginResponseDto);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("CustomError", responseDto?.Message);

            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = GetRoleList();
            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto registerationRequestDto)
        {
            var createUserResult = await _authService.RegisterAsync(registerationRequestDto);
            if (createUserResult != null && createUserResult.IsSuccess)
            {
                registerationRequestDto.Role = !string.IsNullOrEmpty(registerationRequestDto.Role) ? registerationRequestDto.Role : SD.RoleCustomer;
                var assignRoleResult = await _authService.AssignRoleAsync(registerationRequestDto);
                if (assignRoleResult != null && assignRoleResult.IsSuccess)
                {
                    TempData["success"] = "Register new user successfully";
                    return Redirect(nameof(Login));
                }
                else
                {
                    TempData["error"] = assignRoleResult?.Message;
                }
            }
            else
            {
                TempData["error"] = createUserResult?.Message;
            }            

            var roleList = GetRoleList();
            ViewBag.RoleList = roleList;

            return View(registerationRequestDto);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private List<SelectListItem> GetRoleList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = SD.RoleAdmin,
                    Value = SD.RoleAdmin,
                },
                new SelectListItem
                {
                    Text = SD.RoleCustomer,
                    Value = SD.RoleCustomer,
                }
            };
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
