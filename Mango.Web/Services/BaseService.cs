using Mango.Web.Models;
using Mango.Web.Services.IService;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto request)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                // token

                message.RequestUri = new Uri(request.Url);
                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse;

                switch (request.ApiType)
                {
                    case Ultility.SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Ultility.SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Ultility.SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new ResponseDto { IsSuccess = false, Message = "Not found" };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new ResponseDto { IsSuccess = false, Message = "Forbidden" };
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new ResponseDto { IsSuccess = false, Message = "Unauthorized" };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new ResponseDto { IsSuccess = false, Message = "Internal Server Error" };
                    case System.Net.HttpStatusCode.OK:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    default:
                        return new ResponseDto { IsSuccess = false, Message = "Something went wrong!" };
                }
            }
            catch (Exception e)
            {
                return new ResponseDto { IsSuccess = false, Message = e.Message };
            }
        }
    }
}
