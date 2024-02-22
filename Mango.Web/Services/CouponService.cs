using Mango.Web.Models;
using Mango.Web.Services.IService;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.GET,
                Url = Ultility.SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.GET,
                Url = Ultility.SD.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto?> GetCouponByCodeAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.GET,
                Url = Ultility.SD.CouponAPIBase + "/api/coupon/GetByCode/" + code
            });
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto request)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.POST,
                Url = Ultility.SD.CouponAPIBase + "/api/coupon",
                Data = request
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto request)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.PUT,
                Url = Ultility.SD.CouponAPIBase + "/api/coupon",
                Data = request
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Ultility.SD.ApiType.DELETE,
                Url = Ultility.SD.CouponAPIBase + "/api/coupon/" + id
            });
        }
    }
}
