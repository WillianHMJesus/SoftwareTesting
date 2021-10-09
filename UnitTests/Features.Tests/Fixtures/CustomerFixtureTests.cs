using Xunit;

namespace Features.Tests.Fixtures
{
    public class CustomerFixtureTests : IClassFixture<CustomerFixture>
    {
        private readonly CustomerFixture _customerFixture;

        public CustomerFixtureTests(CustomerFixture customerFixture)
        {
            _customerFixture = customerFixture;
        }

        [Fact(DisplayName = "New Customer Valid")]
        [Trait("Category", "Customer Fixture Tests")]
        public void Customer_NewCustomer_MustIsValid()
        {
            //Arrange
            var customer = _customerFixture.GenerateValidCustomer();

            //Act
            var result = customer.IsValid();

            //Assert
            Assert.True(result);
            Assert.Empty(customer.ValidationResult.Errors);
        }

        [Fact(DisplayName = "New Customer Invalid")]
        [Trait("Category", "Customer Fixture Tests")]
        public void Customer_NewCustomer_MustIsInvalid()
        {
            //Arrange
            var customer = _customerFixture.GenerateInvalidCustomer();

            //Act
            var result = customer.IsValid();

            //Assert
            Assert.False(result);
            Assert.NotEmpty(customer.ValidationResult.Errors);
        }
    }
}
