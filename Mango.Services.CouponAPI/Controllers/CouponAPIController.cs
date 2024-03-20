using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;


namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly CouponDbContext _context;
        private readonly IMapper _mapper;

        public CouponAPIController(CouponDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                var coupons = _context.Coupons.ToList();
                var data = _mapper.Map<List<CouponDto>>(coupons);
                return new ResponseDto
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch (Exception e)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        [HttpGet("{id}")]
        public ResponseDto Get(int id)
        {
            try
            {
                var coupon = _context.Coupons.FirstOrDefault(x => x.CouponId == id);
                if (coupon is null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }

                return new ResponseDto
                {
                    IsSuccess = true,
                    Data = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception e)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        [HttpGet("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                var coupon = _context.Coupons.FirstOrDefault(x => x.CouponCode == code);
                if (coupon is null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }

                return new ResponseDto
                {
                    IsSuccess = true,
                    Data = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception e)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        [HttpPost]
        public ResponseDto Create([FromBody] CouponDto couponRequest)
        {
            try
            {
                var couponCodeExist = _context.Coupons.FirstOrDefault(x => x.CouponCode == couponRequest.CouponCode);
                if (couponCodeExist is not null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Code is existed"
                    };
                }

                var coupon = _mapper.Map<Coupon>(couponRequest);
                _context.Coupons.Add(coupon);
                _context.SaveChanges();

                return new ResponseDto
                {
                    IsSuccess = true,
                    Data = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception e)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        [HttpPut]
        public ResponseDto Update([FromBody] CouponDto couponRequest)
        {
            try
            {
                var coupon = _context.Coupons.FirstOrDefault(x => x.CouponId == couponRequest.CouponId);
                if (coupon is null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Coupon does not exist"
                    };
                }

                coupon.CouponCode = couponRequest.CouponCode;
                coupon.DiscountAmount = couponRequest.DiscountAmount;
                coupon.MinAmount = couponRequest.MinAmount;
                _context.Coupons.Update(coupon);
                _context.SaveChanges();

                return new ResponseDto
                {
                    IsSuccess = true,
                    Data = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception e)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        [HttpDelete("{id}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                var coupon = _context.Coupons.FirstOrDefault(x => x.CouponId == id);
                if (coupon is null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Coupon does not exist"
                    };
                }

                _context.Coupons.Remove(coupon);
                _context.SaveChanges();

                return new ResponseDto
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
    }
}
