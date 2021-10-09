using Features.Customers;
using Features.Tests.HumanData;
using MediatR;
using Moq;
using Moq.AutoMock;
using System.Linq;
using System.Threading;
using Xunit;

namespace Features.Tests.AutoMock
{
    [Collection(nameof(CustomerCollection))]
    public class CustomerServiceAutoMockTests
    {
        private readonly CustomerBogusFixture _customerFixture;
        private readonly AutoMocker _mocker;
        private readonly CustomerService _customerService;

        public CustomerServiceAutoMockTests(CustomerBogusFixture customerFixture = null)
        {
            _customerFixture = customerFixture;
            _mocker = new AutoMocker();
            _customerService = _mocker.CreateInstance<CustomerService>();
        }

        [Fact(DisplayName = "Add Customer Successfully")]
        [Trait("Category", "Customer Service Auto Mock Tests")]
        public void CustomerService_Add_MustRunSuccessfully()
        {
            //Arrange
            var customer = _customerFixture.GenerateValidCustomer();

            //Act
            _customerService.Add(customer);

            //Assert
            _mocker.GetMock<ICustomerRepository>().Verify(x => x.Add(customer), Times.Once);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Add Failed Customer")]
        [Trait("Category", "Customer Service Auto Mock Tests")]
        public void CustomerService_Add_MustRunWithFailure()
        {
            //Arrange
            var customer = _customerFixture.GenerateInvalidCustomer();

            //Act
            _customerService.Add(customer);

            //Assert
            _mocker.GetMock<ICustomerRepository>().Verify(x => x.Add(customer), Times.Never);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Active Customers")]
        [Trait("Category", "Customer Service Auto Mock Tests")]
        public void CustomerService_GetAllAssets_MustReturnOnlyActiveCustomers()
        {
            //Arrange
            _mocker.GetMock<ICustomerRepository>().Setup(x => x.GetAll()).Returns(_customerFixture.GenerateVariedCustomers());

            //Act
            var customers = _customerService.GetAllAssets();

            //Assert
            _mocker.GetMock<ICustomerRepository>().Verify(x => x.GetAll(), Times.Once);
            Assert.True(customers.Any());
            Assert.False(customers.Count(x => !x.Active) > 0);
        }
    }
}
