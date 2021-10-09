using Features.Customers;
using Features.Tests.HumanData;
using FluentAssertions;
using FluentAssertions.Extensions;
using MediatR;
using Moq;
using Moq.AutoMock;
using System.Linq;
using System.Threading;
using Xunit;

namespace Features.Tests.FluentAssertions
{
    [Collection(nameof(CustomerCollection))]
    public class CustomerServiceFluentAssertionsTests
    {
        private readonly CustomerBogusFixture _customerFixture;
        private readonly AutoMocker _mocker;
        private readonly CustomerService _customerService;

        public CustomerServiceFluentAssertionsTests(CustomerBogusFixture customerFixture = null)
        {
            _customerFixture = customerFixture;
            _mocker = new AutoMocker();
            _customerService = _mocker.CreateInstance<CustomerService>();
        }

        [Fact(DisplayName = "Get Active Customers")]
        [Trait("Category", "Customer Service Fluent Assertions Tests")]
        public void CustomerService_GetAllAssets_MustReturnOnlyActiveCustomers()
        {
            //Arrange
            _mocker.GetMock<ICustomerRepository>().Setup(x => x.GetAll()).Returns(_customerFixture.GenerateVariedCustomers());

            //Act
            var customers = _customerService.GetAllAssets();

            //Assert
            //_mocker.GetMock<ICustomerRepository>().Verify(x => x.GetAll(), Times.Once);
            //Assert.True(customers.Any());
            //Assert.False(customers.Count(x => !x.Active) > 0);

            //Assert
            customers.Should().HaveCountGreaterOrEqualTo(1, "Deve retornar pelo menos 1 cliente").And
                .OnlyHaveUniqueItems("Não deve retornar clientes duplicados");
            customers.Should().NotContain(x => !x.Active, "Não deve retornar clientes inativos");
            _customerService.ExecutionTimeOf(x => x.GetAllAssets()).Should()
                .BeLessOrEqualTo(50.Milliseconds(), "A execução do método não pode passar de 50 millisegundos");
        }
    }
}
