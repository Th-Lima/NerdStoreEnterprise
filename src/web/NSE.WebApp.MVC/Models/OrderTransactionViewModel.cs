using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using NSE.Core.Validation;

namespace NSE.WebApp.MVC.Models
{
    public class OrderTransactionViewModel
    {
        #region Pedido

        [DisplayName("Valor Total")]
        public decimal TotalValue { get; set; }
        
        [DisplayName("Desconto")]
        public decimal Discount { get; set; }

        [DisplayName("Cõdigo do voucher")]
        public string VoucherCode { get; set; }

        [DisplayName("Voucher utilizado")]
        public bool VoucherUsed { get; set; }

        public List<ItemCartViewModel> Itens { get; set; } = new List<ItemCartViewModel>();

        #endregion

        #region Endereco

        [DisplayName("Endereço")]
        public AddressViewModel Address { get; set; }

        #endregion

        #region Cartão

        [Required(ErrorMessage = "Informe o número do cartão")]
        [DisplayName("Número do Cartão")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Informe o nome do portador do cartão")]
        [DisplayName("Nome do Portador")]
        public string CardName { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "O vencimento deve estar no padrão MM/AA")]
        [CardExpiration(ErrorMessage = "Cartão Expirado")]
        [Required(ErrorMessage = "Informe o vencimento")]
        [DisplayName("Data de Vencimento MM/AA")]
        public string CardExpiration { get; set; }

        [Required(ErrorMessage = "Informe o código de segurança")]
        [DisplayName("Código de Segurança")]
        public string CvvCard { get; set; }

        #endregion
    }
}
