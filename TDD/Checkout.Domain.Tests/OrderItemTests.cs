using Core.DomainObjects;
using System;
using Xunit;

namespace Checkout.Domain.Tests
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "New Order Item With Units Bellow Allowed")]
        [Trait("Category", "Checkout - Order Item")]
        public void AddOrderItem_ItemUnitBellowAllowed_MustReturnException()
        {
            //Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Test Product", Order.MIN_UNITS_ITEM - 1, 100));
        }
    }
}
