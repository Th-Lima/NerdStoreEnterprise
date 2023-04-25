using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Order.API.Application.Dto;
using NSE.Order.API.Application.Queries;
using NSE.WebAPI.Core.Controllers;
using System.Net;
using System.Threading.Tasks;

namespace NSE.Order.API.Controllers
{
    [Authorize]
    public class VoucherController : MainController
    {
        private readonly IVoucherQueries _voucherQueries;

        public VoucherController(IVoucherQueries voucherQuery)
        {
            _voucherQueries = voucherQuery;
        }

        [HttpGet("voucher/{code}")]
        [ProducesResponseType(typeof(VoucherDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code)) 
                return NotFound();

            var voucher = await _voucherQueries.GetVoucherByCode(code);

            return voucher == null ? NotFound() : CustomResponse(voucher);
        }
    }
}
