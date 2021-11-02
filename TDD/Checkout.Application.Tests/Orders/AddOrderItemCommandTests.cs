using Checkout.Application.Commands;
using Checkout.Domain;
using System;
using System.Linq;
using Xunit;

namespace Checkout.Application.Tests.Orders
{
    public class AddOrderItemCommandTests
    {
        [Fact(DisplayName = "Add Item Command Valid")]
        [Trait("Category", "Checkout - Order Commands")]
        public void AddOrderItemCommand_CommandIsValid_MustPassValidation()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

            //Act
            var result = orderCommand.IsValid();

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Add Item Command Invalid")]
        [Trait("Category", "Checkout - Order Commands")]
        public void AddOrderItemCommand_CommandIsInvalid_MustNotPassValidation()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            //Act
            var result = orderCommand.IsValid();

            //Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.CustomerIdErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.ProductIdErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.NameErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.MinimumQuantityErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.UnitValueErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Add Item Command Units Above Allowed")]
        [Trait("Category", "Checkout - Order Commands")]
        public void AddOrderItemCommand_QuantityUnitAboveAllowed_MustNotPassValidation()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", Order.MAX_UNITS_ITEM + 1, 100);

            //Act
            var result = orderCommand.IsValid();

            //Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.MaximumQuantityErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
        }
    }
}
