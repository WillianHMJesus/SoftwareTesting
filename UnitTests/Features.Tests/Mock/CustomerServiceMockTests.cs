using Features.Customers;
using Features.Tests.HumanData;
using MediatR;
using Moq;
using System.Linq;
using System.Threading;
using Xunit;

namespace Features.Tests.Mock
{
    [Collection(nameof(CustomerCollection))]
    public class CustomerServiceMockTests
    {
        private readonly CustomerBogusFixture _customerFixture;

        public CustomerServiceMockTests(CustomerBogusFixture customerFixture = null)
        {
            _customerFixture = customerFixture;
        }

        [Fact(DisplayName = "Add Customer Successfully")]
        [Trait("Category", "Customer Service Mock Tests")]
        public void CustomerService_Add_MustRunSuccessfully()
        {
            //Arrange
            var customer = _customerFixture.GenerateValidCustomer();
            var customerRepository = new Mock<ICustomerRepository>();
            var mediator = new Mock<IMediator>();
            var customerService = new CustomerService(customerRepository.Object, mediator.Object);

            //Act
            customerService.Add(customer);

            //Assert
            customerRepository.Verify(x => x.Add(customer), Times.Once);
            mediator.Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Add Failed Customer")]
        [Trait("Category", "Customer Service Mock Tests")]
        public void CustomerService_Add_MustRunWithFailure()
        {
            //Arrange
            var customer = _customerFixture.GenerateInvalidCustomer();
            var customerRepository = new Mock<ICustomerRepository>();
            var mediator = new Mock<IMediator>();
            var customerService = new CustomerService(customerRepository.Object, mediator.Object);

            //Act
            customerService.Add(customer);

            //Assert
            customerRepository.Verify(x => x.Add(customer), Times.Never);
            mediator.Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Active Customers")]
        [Trait("Category", "Customer Service Mock Tests")]
        public void CustomerService_GetAllAssets_MustReturnOnlyActiveCustomers()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetAll()).Returns(_customerFixture.GenerateVariedCustomers());
            var mediator = new Mock<IMediator>();
            var customerService = new CustomerService(customerRepository.Object, mediator.Object);

            //Act
            var customers = customerService.GetAllAssets();

            //Assert
            customerRepository.Verify(x => x.GetAll(), Times.Once);
            Assert.True(customers.Any());
            Assert.False(customers.Count(x => !x.Active) > 0);
        }
    }
}
