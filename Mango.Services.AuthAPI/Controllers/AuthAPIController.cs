using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto registerationRequestDto)
        {
            var errorMessage = await _authService.Register(registerationRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var result = await _authService.Login(loginRequestDto);
            if (result.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username of password is invalid";
                return BadRequest(_responseDto);
            }

            _responseDto.Data = result;
            return Ok(_responseDto);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDto registerationRequestDto)
        {
            if (string.IsNullOrEmpty(registerationRequestDto.Role))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Enter role name!";
                return BadRequest(_responseDto);
            }

            var result = await _authService.AssignRole(registerationRequestDto.Email, registerationRequestDto.Role.ToUpper());
            if (!result)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Assign role failed";
                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }
    }
}
