using Features.Tests.HumanData;
using FluentAssertions;
using Xunit;

namespace Features.Tests.FluentAssertions
{
    [Collection(nameof(CustomerCollection))]
    public class CustomerFluentAssertionsTests
    {
        private readonly CustomerBogusFixture _customerFixture;

        public CustomerFluentAssertionsTests(CustomerBogusFixture customerFixture = null)
        {
            _customerFixture = customerFixture;
        }

        [Fact(DisplayName = "New Customer Valid")]
        [Trait("Category", "Customer Fluent Assertions Tests")]
        public void Customer_NewCustomer_MustIsValid()
        {
            //Arrange
            var customer = _customerFixture.GenerateValidCustomer();

            //Act
            var result = customer.IsValid();

            //Assert
            //Assert.True(result);
            //Assert.Empty(customer.ValidationResult.Errors);

            //Assert
            result.Should().BeTrue("O resultado deve ser verdadeiro");
            customer.ValidationResult.Errors.Should().HaveCount(0, "Não deve possuir erros de validação");
        }

        [Fact(DisplayName = "New Customer Invalid")]
        [Trait("Category", "Customer Fluent Assertions Tests")]
        public void Customer_NewCustomer_MustIsInvalid()
        {
            //Arrange
            var customer = _customerFixture.GenerateInvalidCustomer();

            //Act
            var result = customer.IsValid();

            //Assert
            //Assert.False(result);
            //Assert.NotEmpty(customer.ValidationResult.Errors);

            //Assert
            result.Should().BeFalse("O resultado deve ser falso");
            customer.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1, "Deve possuir erros de validação");
        }
    }
}
