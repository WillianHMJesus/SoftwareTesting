using Checkout.Application.Events;
using Checkout.Domain;
using Checkout.Domain.Interfaces;
using Core.DomainObjects;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Commands
{
    public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
            {
                return false;
            }

            var order = await _orderRepository.GetOrderDraftByCustomerId(message.CustomerId);
            var orderItem = new OrderItem(message.ProductId, message.Name, message.Quantity, message.UnitValue);

            if (order == null)
            {
                AddItemNewOrder(ref order, orderItem, message);
            }
            else
            {
                AddItemExistingOrder(order, orderItem);
            }

            order.AddEvent(new OrderItemAddedEvent(
                order.CustomerId,
                order.Id,
                message.ProductId,
                message.Name,
                message.UnitValue,
                message.Quantity));

            return await _orderRepository.UnitOfWork.Commit();
        }

        private bool ValidateCommand(AddOrderItemCommand message)
        {
            if (message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediator.Publish(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }

        private void AddItemNewOrder(ref Order order, OrderItem orderItem, AddOrderItemCommand message)
        {
            order = Order.OrderFactory.NewDraftOrder(message.CustomerId);
            order.AddItem(orderItem);

            _orderRepository.Add(order);
        }

        private void AddItemExistingOrder(Order order, OrderItem orderItem)
        {
            var existingOrderItem = order.ExistingOrderItem(orderItem);
            order.AddItem(orderItem);

            if (existingOrderItem)
            {
                _orderRepository.UpdateItem(order.OrderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId));
            }
            else
            {
                _orderRepository.AddItem(orderItem);
            }

            _orderRepository.Update(order);
        }
    }
}
