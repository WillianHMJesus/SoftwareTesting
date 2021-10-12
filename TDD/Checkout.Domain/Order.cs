using Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Domain
{
    public class Order
    {
        public static int MAX_UNITS_ITEM = 15;
        public static int MIN_UNITS_ITEM = 1;
        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public Guid CustomerId { get; private set; }
        public decimal Amount { get; private set; }
        public StatusOrder StatusOrder { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public void AddItem(OrderItem orderItem)
        {
            ValidateItemQuantityAllowed(orderItem);

            if (ExistingOrderItem(orderItem))
            {
                var exitingItem = _orderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);
                exitingItem.AddUnits(orderItem.Quantity);
                orderItem = exitingItem;

                _orderItems.Remove(exitingItem);
            }

            _orderItems.Add(orderItem);
            CalculateOrderAmount();
        }

        public void UpdateItem(OrderItem orderItem)
        {
            ValidateInexistentOrderItem(orderItem);
            ValidateItemQuantityAllowed(orderItem);

            var existingItem = OrderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);
            _orderItems.Remove(existingItem);
            _orderItems.Add(orderItem);
            CalculateOrderAmount();
        }

        public void RemoveItem(OrderItem orderItem)
        {
            ValidateInexistentOrderItem(orderItem);

            _orderItems.Remove(orderItem);
            CalculateOrderAmount();
        }

        private void CalculateOrderAmount()
        {
            Amount = OrderItems.Sum(x => x.CalculateValue());
        }

        private bool ExistingOrderItem(OrderItem orderItem)
        {
            return _orderItems.Any(x => x.ProductId == orderItem.ProductId);
        }

        private void ValidateItemQuantityAllowed(OrderItem orderItem)
        {
            var itemsQuantity = orderItem.Quantity;
            if(ExistingOrderItem(orderItem))
            {
                var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);
                itemsQuantity += existingItem.Quantity;
            }

            if (itemsQuantity > MAX_UNITS_ITEM)
                throw new DomainException($"Máximo de {MAX_UNITS_ITEM} unidades por produto");
        }

        private void ValidateInexistentOrderItem(OrderItem orderItem)
        {
            if (!ExistingOrderItem(orderItem))
                throw new DomainException("O item não existe no pedido");
        }

        private void SetDraftStatus()
        {
            StatusOrder = StatusOrder.Draft;
        }

        public static class OrderFactory
        {
            public static Order NewDraftOrder(Guid customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId
                };

                order.SetDraftStatus();
                return order;
            }
        }
    }
}
