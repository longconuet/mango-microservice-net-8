using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Services.IService
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(ApplicationUser applicationUser);
    }
}
