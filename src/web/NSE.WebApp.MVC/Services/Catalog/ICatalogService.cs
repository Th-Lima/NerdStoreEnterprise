using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Catalog
{
    [Obsolete("Esta interface não está sendo utilizada de fato, pois estamos fazendo o processo de Http Service com o Refit. Utilizando a interface Refit.Catalog.ICatalogServiceRefit")]
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<ProductViewModel> GetById(Guid id);
    }
}
