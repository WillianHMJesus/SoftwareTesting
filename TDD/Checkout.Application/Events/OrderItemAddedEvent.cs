using Core.Messages;
using System;

namespace Checkout.Application.Events
{
    public class OrderItemAddedEvent : Event
    {
        public OrderItemAddedEvent(Guid customerId, Guid orderId, Guid productId, string productName, decimal unitValue, int quantity)
        {
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            UnitValue = unitValue;
            Quantity = quantity;
        }

        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public Guid ProductId { get; }
        public string ProductName { get; }
        public decimal UnitValue { get; }
        public int Quantity { get; }
    }
}
