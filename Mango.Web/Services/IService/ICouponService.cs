using Mango.Web.Models;

namespace Mango.Web.Services.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> GetCouponByCodeAsync(string code);
        Task<ResponseDto?> CreateCouponAsync(CouponDto request);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto request);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
