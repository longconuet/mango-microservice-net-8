using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Services.IService;

namespace Mango.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegisterationRequestDto registerationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.POST,
                Url = Ultility.SD.AuthAPIBase + "/api/auth/assign-role",
                Data = registerationRequestDto
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.POST,
                Url = Ultility.SD.AuthAPIBase + "/api/auth/login",
                Data = loginRequestDto
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegisterationRequestDto registerationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.POST,
                Url = Ultility.SD.AuthAPIBase + "/api/auth/register",
                Data = registerationRequestDto
            });
        }
    }
}
