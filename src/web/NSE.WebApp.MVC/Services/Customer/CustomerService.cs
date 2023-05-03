using Microsoft.Extensions.Options;
using NSE.Core.Communication;
using NSE.WebAPI.Core.Identity;
using NSE.WebApp.MVC.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Customer
{
    public class CustomerService : Service, ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
        }

        public async Task<AddressViewModel> GetAddress()
        {
            var response = await _httpClient.GetAsync("/customer/address/");

            if (response.StatusCode == HttpStatusCode.NotFound) 
                return null;

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<AddressViewModel>(response);
        }

        public async Task<ResponseResult> AddAddress(AddressViewModel address)
        {
            var addressContent = GetContent(address);

            var response = await _httpClient.PostAsync("/customer/address/", addressContent);

            if (!HandleErrorResponse(response)) 
                return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
