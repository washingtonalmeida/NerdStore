using MediatR;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Sales.Application.Commands.Models;
using NerdStore.Sales.Application.Events.Models;
using NerdStore.Sales.Domain.Interfaces;
using NerdStore.Sales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands.Handlers
{
    public class ApplyVoucherToOrderCommandHandler : IRequestHandler<ApplyVoucherToOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public ApplyVoucherToOrderCommandHandler(IOrderRepository orderRepository, IMediatrHandler mediatrHandler)
        {
            _orderRepository = orderRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(ApplyVoucherToOrderCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await PublishCommandErrorsNotifications(message);
                return false;
            }

            var draftOrder = GetDraftOrderStarted(message.CustomerId);
            if (draftOrder == null)
            {
                await PublishOrderNotFoundNotification();
                return false;
            }

            var voucher = GetVoucher(message.VoucherCode);
            if (voucher == null)
            {
                await PublishVoucherNotFoundNotification();
                return false;
            }

            if (!IsVoucherValid(voucher))
            {
                await PublishVoucherErrorsNotifications(voucher);
                return false;
            }

            draftOrder.ApplyVoucher(voucher);
            draftOrder.AddEvent(new VoucherAppliedToOrderEvent(draftOrder.CustomerId, draftOrder.Id, voucher.Id));
            draftOrder.AddEvent(new OrderUpdatedEvent(draftOrder.CustomerId, draftOrder.Id, draftOrder.TotalPrice));

            PersistOrderUpdated(draftOrder);
            return await CommitChanges();
        }

        private async Task PublishCommandErrorsNotifications(ApplyVoucherToOrderCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                await _mediatrHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        private Order GetDraftOrderStarted(Guid customerId)
        {
            return _orderRepository.GetDraftOrderByCustomerId(customerId).Result;
        }

        private async Task PublishOrderNotFoundNotification()
        {
            await _mediatrHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
        }

        private Voucher GetVoucher(string voucherCode)
        {
            return _orderRepository.GetVoucherByCode(voucherCode).Result;
        }

        private async Task PublishVoucherNotFoundNotification()
        {
            await _mediatrHandler.PublishNotification(new DomainNotification("order", "Voucher not found!"));
        }

        private bool IsVoucherValid(Voucher voucher)
        {
            return GetVoucherErrors(voucher).Count == 0;
        }

        private async Task PublishVoucherErrorsNotifications(Voucher voucher)
        {
            var voucherErrors = GetVoucherErrors(voucher);

            foreach (var errorMessage in voucherErrors)
            {
                await _mediatrHandler.PublishNotification(new DomainNotification("order", errorMessage));
            }
        }

        private void PersistOrderUpdated(Order order)
        {
            _orderRepository.Update(order);
        }

        private async Task<bool> CommitChanges()
        {
            return await _orderRepository.UnitOfWork.Commit();
        }

        private List<string> GetVoucherErrors(Voucher voucher)
        {
            var voucherErrors = new List<string>();

            if (voucher.IsExpired())
                voucherErrors.Add("The voucher is expired. ");

            if (!voucher.HasQuantityAvailable())
                voucherErrors.Add("The voucher is not available.");

            if (voucher.IsUsed())
                voucherErrors.Add("The voucher has already been used.");

            if (!voucher.IsActive())
                voucherErrors.Add("The voucher is no longer valid.");

            return voucherErrors;
        }
    }
}
