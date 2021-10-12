using Core.DomainObjects;
using System;

namespace Checkout.Domain
{
    public class OrderItem
    {
        public OrderItem(Guid productId, string productName, int quantity, decimal unitValue)
        {
            if (quantity < Order.MIN_UNITS_ITEM)
                throw new DomainException($"Mínimo de {Order.MIN_UNITS_ITEM} unidades por produto");

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitValue { get; private set; }

        internal void AddUnits(int units)
        {
            Quantity += units;
        }

        internal decimal CalculateValue()
        {
            return Quantity * UnitValue;
        }
    }
}
