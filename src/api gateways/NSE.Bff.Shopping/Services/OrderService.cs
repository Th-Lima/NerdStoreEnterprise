using Microsoft.Extensions.Options;
using NSE.Bff.Shopping.Extensions;
using NSE.Bff.Shopping.Models;
using NSE.Core.Communication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Services
{
    public interface IOrderService
    {
        Task<ResponseResult> FinalizeOrder(OrderDto pedido);
        Task<OrderDto> GetLastOrder();
        Task<IEnumerable<OrderDto>> GetListByCustomerId();

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

        public async Task<ResponseResult> FinalizeOrder(OrderDto order)
        {
            var orderContent = GetContent(order);

            var response = await _httpClient.PostAsync("/order/", orderContent);

            if (!HandleErrorsResponse(response)) 
                return await DeserializarObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<OrderDto> GetLastOrder()
        {
            var response = await _httpClient.GetAsync("/order/last/");

            if (response.StatusCode == HttpStatusCode.NotFound) 
                return null;

            HandleErrorsResponse(response);

            return await DeserializarObjectResponse<OrderDto>(response);
        }

        public async Task<IEnumerable<OrderDto>> GetListByCustomerId()
        {
            var response = await _httpClient.GetAsync("/order/list-client/");

            if (response.StatusCode == HttpStatusCode.NotFound) 
                return null;

            HandleErrorsResponse(response);

            return await DeserializarObjectResponse<IEnumerable<OrderDto>>(response);
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
