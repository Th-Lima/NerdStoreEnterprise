﻿using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/catalog/products");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<IEnumerable<ProductViewModel>>(response);
        }

        public async Task<ProductViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{id}");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<ProductViewModel>(response);
        }
    }
}