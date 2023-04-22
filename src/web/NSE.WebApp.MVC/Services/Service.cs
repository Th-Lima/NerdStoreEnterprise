using NSE.Core.Communication;
using NSE.WebApp.MVC.Extensions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace NSE.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected StringContent GetContent(object data)
        {
            return new StringContent(
                JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage reponseMessage) 
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(await reponseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool HandleErrorResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }
        protected ResponseResult ReturnOk()
        {
            return new ResponseResult();
        }
    }
}
