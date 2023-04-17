using Microsoft.AspNetCore.Authorization;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Cart.API.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
    }
}
