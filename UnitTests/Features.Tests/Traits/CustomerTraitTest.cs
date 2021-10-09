using Features.Customers;
using System;
using Xunit;

namespace Features.Tests.Traits
{
    public class CustomerTraitTest
    {
        [Fact(DisplayName = "New Customer Valid")]
        [Trait("Category", "Customer Trait Tests")]
        public void Customer_NewCustomer_MustIsValid()
        {
            //Arrange
            var customer = new Customer(
                Guid.NewGuid(),
                "Willian",
                "Jesus",
                DateTime.Now.AddYears(-30),
                "willian@jesus.com",
                DateTime.Now,
                true);

            //Act
            var result = customer.IsValid();

            //Assert
            Assert.True(result);
            Assert.Empty(customer.ValidationResult.Errors);
        }

        [Fact(DisplayName = "New Customer Invalid")]
        [Trait("Category", "Customer Trait Tests")]
        public void Customer_NewCustomer_MustIsInvalid()
        {
            //Arrange
            var customer = new Customer(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "willian2jesus.com",
                DateTime.Now,
                true);

            //Act
            var result = customer.IsValid();

            //Assert
            Assert.False(result);
            Assert.NotEmpty(customer.ValidationResult.Errors);
        }
    }
}
