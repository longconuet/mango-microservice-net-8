using Mango.Web.Models.Dto;
using Mango.Web.Services.IService;
using Mango.Web.Ultility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

        public IActionResult Logout()
        {
            return View();
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
    }
}
