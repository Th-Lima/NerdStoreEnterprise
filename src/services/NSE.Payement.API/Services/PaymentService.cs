using FluentValidation.Results;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.Payment.API.Facade;
using NSE.Payment.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Payment.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentFacade _paymentFacade;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentFacade paymentFacade,
                                IPaymentRepository paymentRepository)
        {
            _paymentFacade = paymentFacade;
            _paymentRepository = paymentRepository;
        }

        public async Task<ResponseMessage> AuthorizePayment(Models.Payment payment)
        {
            var transaction = await _paymentFacade.AuthorizePayment(payment);
            var validationResult = new ValidationResult();

            if (transaction.Status != StatusTransaction.Authorized)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                        "Pagamento recusado, entre em contato com a sua operadora de cartão"));

                return new ResponseMessage(validationResult);
            }

            payment.AddTransaction(transaction);
            _paymentRepository.AddPayment(payment);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    "Houve um erro ao realizar o pagamento."));

                // Cancelar pagamento no gateway
                await CancelPayment(payment.OrderId);

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CapturePayment(Guid orderId)
        {
            var transactionsList = await _paymentRepository.GetTransactionsByOrderId(orderId);
            var transactionAuthorize = transactionsList?.FirstOrDefault(t => t.Status == StatusTransaction.Authorized);
            var validationResult = new ValidationResult();

            if (transactionAuthorize == null) 
                throw new DomainException($"Transação não encontrada para o pedido {orderId}");

            var transaction = await _paymentFacade.CapturePayment(transactionAuthorize);

            if (transaction.Status != StatusTransaction.Paid)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível capturar o pagamento do pedido {orderId}"));

                return new ResponseMessage(validationResult);
            }

            transaction.PaymentId = transactionAuthorize.PaymentId;
            _paymentRepository.AddTransaction(transaction);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível persistir a captura do pagamento do pedido {orderId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CancelPayment(Guid orderId)
        {
            var transactionsList = await _paymentRepository.GetTransactionsByOrderId(orderId);
            var transactionAuthorize = transactionsList?.FirstOrDefault(t => t.Status == StatusTransaction.Authorized);
            var validationResult = new ValidationResult();

            if (transactionAuthorize == null) throw new DomainException($"Transação não encontrada para o pedido {orderId}");

            var transaction = await _paymentFacade.CancelAuthorization(transactionAuthorize);

            if (transaction.Status != StatusTransaction.Cancelled)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível cancelar o pagamento do pedido {orderId}"));

                return new ResponseMessage(validationResult);
            }

            transaction.PaymentId = transactionAuthorize.PaymentId;
            _paymentRepository.AddTransaction(transaction);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível persistir o cancelamento do pagamento do pedido {orderId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}
