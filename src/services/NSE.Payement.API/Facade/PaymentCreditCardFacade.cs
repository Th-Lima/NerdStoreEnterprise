using Microsoft.Extensions.Options;
using NSE.Pagamentos.NerdsPag;
using NSE.Payment.API.Models;
using System;
using System.Threading.Tasks;

namespace NSE.Payment.API.Facade
{
    public class PaymentCreditCardFacade : IPaymentFacade
    {
        private readonly PaymentConfig _paymentConfig;

        public PaymentCreditCardFacade(IOptions<PaymentConfig> paymentConfig)
        {
            _paymentConfig = paymentConfig.Value;
        }

        public async Task<Models.Transaction> AuthorizePayment(Models.Payment payment)
        {
            var nerdsPaySvc = CreateNerdsPayInstance();

            var cardHashGen = new CardHash(nerdsPaySvc)
            {
                CardNumber = payment.CreditCard.CardNumber,
                CardHolderName = payment.CreditCard.CardName,
                CardExpirationDate = payment.CreditCard.MonthYearDue,
                CardCvv = payment.CreditCard.CVV
            };

            var cardHash = cardHashGen.Generate();

            var transaction = new NSE.Pagamentos.NerdsPag.Transaction(nerdsPaySvc)
            {
                CardHash = cardHash,
                CardNumber = payment.CreditCard.CardNumber,
                CardHolderName = payment.CreditCard.CardName,
                CardExpirationDate = payment.CreditCard.MonthYearDue,
                CardCvv = payment.CreditCard.CVV,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = payment.TotalValue
            };

            return ForTransactionModel(await transaction.AuthorizeCardTransaction());
        }

        public async Task<Models.Transaction> CapturePayment(Models.Transaction transaction)
        {
            var nerdsPagSvc = CreateNerdsPayInstance();

            var forTransaction = ForTransactionLibNerdsPay(transaction, nerdsPagSvc);

            return ForTransactionModel(await forTransaction.CaptureCardTransaction());
        }

        public async Task<Models.Transaction> CancelAuthorization(Models.Transaction transacao)
        {
            var nerdsPagSvc = CreateNerdsPayInstance();

            var transaction = ForTransactionLibNerdsPay(transacao, nerdsPagSvc);

            return ForTransactionModel(await transaction.CancelAuthorization());
        }

        /// <summary>
        /// (De-Para) De Transaction da lib NerdsPay  para Transaction da Model dentro da API
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="nerdsPayService"></param>
        /// <returns>Retorna uma instância de Transaction da Lib NerdsPay</returns>
        public static Models.Transaction ForTransactionModel(NSE.Pagamentos.NerdsPag.Transaction transaction)
        {
            return new Models.Transaction
            {
                Id = Guid.NewGuid(),
                Status = (StatusTransaction)transaction.Status,
                TotalValue = transaction.Amount,
                CardBrand = transaction.CardBrand,
                AuthorizationCode = transaction.AuthorizationCode,
                TransactionCost = transaction.Cost,
                TransactionDate = transaction.TransactionDate,
                NSU = transaction.Nsu,
                TID = transaction.Tid
            };
        }

        /// <summary>
        /// (De-Para) De Transaction do Model dentro da API para Transaction da lib NerdsPay 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="nerdsPayService"></param>
        /// <returns>Retorna uma instância de Transaction da Lib NerdsPay</returns>
        public static NSE.Pagamentos.NerdsPag.Transaction ForTransactionLibNerdsPay(Models.Transaction transaction, NerdsPayService nerdsPayService)
        {
            return new NSE.Pagamentos.NerdsPag.Transaction(nerdsPayService)
            {
                Status = (TransactionStatus)transaction.Status,
                Amount = transaction.TotalValue,
                CardBrand = transaction.CardBrand,
                AuthorizationCode = transaction.AuthorizationCode,
                Cost = transaction.TransactionCost,
                Nsu = transaction.NSU,
                Tid = transaction.TID
            };
        }

        private NerdsPayService CreateNerdsPayInstance()
        {
            return new NerdsPayService(_paymentConfig.DefaultApiKey, _paymentConfig.DefaultEncryptionKey);
        }
    }
}
