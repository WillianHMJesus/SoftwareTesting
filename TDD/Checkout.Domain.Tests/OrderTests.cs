using Core.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace Checkout.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add New Order Item")]
        [Trait("Category", "Checkout - Order")]
        public void AddOrderItem_NewOrder_MustUpdateAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);

            //Act
            order.AddItem(orderItem);

            //Assert
            Assert.Equal(200, order.Amount);
        }

        [Fact(DisplayName = "Add Existing Order Item")]
        [Trait("Category", "Checkout - Order")]
        public void AddOrderItem_ExistingItem_MustAddUnitsAndSumValues()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Test Product", 1, 100);

            //Act
            order.AddItem(orderItem2);

            //Assert
            Assert.Equal(300, order.Amount);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(3, order.OrderItems.FirstOrDefault(x => x.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Add Order Item Above Allowed")]
        [Trait("Category", "Checkout - Order")]
        public void AddOrderItem_ItemUnitAboveAllowed_MustReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", Order.MAX_UNITS_ITEM + 1, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem));
        }

        [Fact(DisplayName = "Add Existing Order Item Above Allowed")]
        [Trait("Category", "Checkout - Order")]
        public void AddOrderItem_ExistingItemSumUnitAboveAllowed_MustReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 1, 100);
            var orderItem2 = new OrderItem(productId, "Test Product", Order.MAX_UNITS_ITEM, 100);
            order.AddItem(orderItem);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
        }

        [Fact(DisplayName = "Update Inexistent Order Item")]
        [Trait("Category", "Checkout - Order")]
        public void UpdateOrderItem_ItemNotExistInList_MustReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var updatedOrderItem = new OrderItem(Guid.NewGuid(), "Test Product", 5, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(updatedOrderItem));
        }

        [Fact(DisplayName = "Update Valid Order Item")]
        [Trait("Category", "Checkout - Order")]
        public void UpdateOrderItem_ValidItem_MustUpdateQuantity()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 2, 100);
            order.AddItem(orderItem);
            var updatedOrderItem = new OrderItem(productId, "Test Product", 5, 100);
            var newQuantity = updatedOrderItem.Quantity;

            //Act
            order.UpdateItem(updatedOrderItem);

            //Assert
            Assert.Equal(newQuantity, order.OrderItems.FirstOrDefault(x => x.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Update Order Item Validate Amount")]
        [Trait("Category", "Checkout - Order")]
        public void UpdateOrderItem_OrderWithDifferentProducts_MustUpdateAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var existingOrderItem1 = new OrderItem(Guid.NewGuid(), "Xpto Product", 2, 100);
            var existingOrderItem2 = new OrderItem(productId, "Test Product", 3, 15);
            order.AddItem(existingOrderItem1);
            order.AddItem(existingOrderItem2);

            var updatedOrderItem = new OrderItem(productId, "Test Product", 5, 15);
            var amount = existingOrderItem1.Quantity * existingOrderItem1.UnitValue +
                         updatedOrderItem.Quantity * updatedOrderItem.UnitValue;

            //Act
            order.UpdateItem(updatedOrderItem);

            //Assert
            Assert.Equal(amount, order.Amount);
        }

        [Fact(DisplayName = "Update Order Item Quantity Above Allowed")]
        [Trait("Category", "Checkout - Order")]
        public void UpdateOrderItem_UnitsItemAboveAllowed_MustReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var existingOrderItem = new OrderItem(productId, "Test Product", 3, 15);
            order.AddItem(existingOrderItem);

            var updatedOrderItem = new OrderItem(productId, "Test Product", Order.MAX_UNITS_ITEM + 1, 15);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(updatedOrderItem));
        }

        [Fact(DisplayName = "Remove Inexistent Order Item")]
        [Trait("Category", "Checkout - Order")]
        public void RemoveOrderItem_ItemNotExistInList_MustReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var removedOrderItem = new OrderItem(Guid.NewGuid(), "Test Product", 5, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.RemoveItem(removedOrderItem));
        }

        [Fact(DisplayName = "Remove Order Item Validate Amount")]
        [Trait("Category", "Checkout - Order")]
        public void RemoveOrderItem_ExistingItem_MustUpdateAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Xpto Product", 2, 100);
            var orderItem2 = new OrderItem(productId, "Test Product", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var amount = orderItem2.Quantity * orderItem2.UnitValue;

            //Act
            order.RemoveItem(orderItem1);

            //Assert
            Assert.Equal(amount, order.Amount);
        }

        [Fact(DisplayName = "Apply Valid Voucher")]
        [Trait("Category", "Checkout - Order")]
        public void ApplyVoucher_ValidVoucher_MustReturnWithoutErrors()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15-REAIS", null, 15, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(7), false, true);

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Apply Invalid Voucher")]
        [Trait("Category", "Checkout - Order")]
        public void ApplyVoucher_InvalidVoucher_MustReturnWithErrors()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var voucher = new Voucher("", null, null, VoucherDiscountType.Value, 0, DateTime.Now.AddDays(-1), true, false);

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Apply Voucher Value Type Valid")]
        [Trait("Category", "Checkout - Order")]
        public void ApplyVoucher_VoucherValueType_MustDiscountAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Xpto Product", 2, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var voucher = new Voucher("PROMO-15-REAIS", null, 15, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(7), false, true);
            var discountAmount = order.Amount - voucher.DiscountValue;

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(discountAmount, order.Amount);
        }

        [Fact(DisplayName = "Apply Voucher Percentage Type Valid")]
        [Trait("Category", "Checkout - Order")]
        public void ApplyVoucher_VoucherPercentageType_MustDiscountAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Xpto Product", 2, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var voucher = new Voucher("PROMO-15-REAIS", 15, null, VoucherDiscountType.Percentage, 1, DateTime.Now.AddDays(7), false, true);
            var discountValue = (order.Amount * voucher.DiscountPercentage) / 100;
            var discountAmount = order.Amount - discountValue;

            //Act
            order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(discountAmount, order.Amount);
        }

        [Fact(DisplayName = "Apply Voucher Discount Above Amount")]
        [Trait("Category", "Checkout - Order")]
        public void ApplyVoucher_DiscountAboveAmount_OrderMustHaveZeroValue()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            order.AddItem(orderItem);

            var voucher = new Voucher("PROMO-300-OFF", null, 300, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(10), false, true);

            //Act
            order.ApplyVoucher(voucher);

            //Arrange
            Assert.Equal(0, order.Amount);
        }

        [Fact(DisplayName = "Apply Voucher Recalculate Discount Order")]
        public void ApplyVoucher_ModifyOrderItems_MustCalculateAmountDiscount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            order.AddItem(orderItem1);

            var voucher = new Voucher("PROMO-50-OFF", null, 50, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(10), false, true);
            order.ApplyVoucher(voucher);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto Teste", 4, 25);

            //Act
            order.AddItem(orderItem2);

            //Assert
            var expectedValue = order.OrderItems.Sum(x => x.Quantity * x.UnitValue) - voucher.DiscountValue;
            Assert.Equal(expectedValue, order.Amount);
        }
    }
}
