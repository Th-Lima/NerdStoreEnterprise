using Microsoft.Extensions.Options;
using NSE.Bff.Shopping.Extensions;
using NSE.Bff.Shopping.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Services
{
    public interface IOrderService
    {
        Task<VoucherDto> GetVoucherByCode(string code);
    }

    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.OrderUrl);
        }

        public async Task<VoucherDto> GetVoucherByCode(string code)
        {
            var response = await _httpClient.GetAsync($"/voucher/{code}/");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            HandleErrorsResponse(response);

            return await DeserializarObjectResponse<VoucherDto>(response);
        }
    }
}
