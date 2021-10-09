using Xunit;

namespace Features.Tests.HumanData
{
    [Collection(nameof(CustomerCollection))]
    public class CustomerBogusTest
    {
        private readonly CustomerBogusFixture _customerFixture;

        public CustomerBogusTest(CustomerBogusFixture customerFixture = null)
        {
            _customerFixture = customerFixture;
        }

        [Fact(DisplayName = "New Customer Valid")]
        [Trait("Category", "Customer Human Data Tests")]
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
        [Trait("Category", "Customer Human Data Tests")]
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
