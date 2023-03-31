using NSE.WebApp.MVC.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Refit.Catalog
{
    [Obsolete("Esta interface não está sendo utilizada de fato, pois estamos fazendo o processo de Http Service de maneira tradicional sem o Refit. Utilizando a interface Catalog.CatalogServiceRefit")]
    public interface ICatalogServiceRefit
    {
        [Get("/catalog/products/")]
        Task<IEnumerable<ProductViewModel>> GetAll();

        [Get("/catalog/products/{id}")]
        Task<ProductViewModel> GetById(Guid id);
    }
}
