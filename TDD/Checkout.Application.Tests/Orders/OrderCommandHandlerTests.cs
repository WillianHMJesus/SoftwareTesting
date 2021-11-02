using Checkout.Application.Commands;
using Checkout.Domain;
using Checkout.Domain.Interfaces;
using MediatR;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Application.Tests.Orders
{
    public class OrderCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly OrderCommandHandler _orderHandler;
        private readonly Guid _customerId;
        private readonly Guid _productId;
        private readonly Order _order;

        public OrderCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _orderHandler = _mocker.CreateInstance<OrderCommandHandler>();

            _customerId = Guid.NewGuid();
            _productId = Guid.NewGuid();
            _order = Order.OrderFactory.NewDraftOrder(_customerId);
        }

        [Fact(DisplayName = "Add New Order Item With Success")]
        [Trait("Category", "Checkout - Order Command Handler")]
        public async Task AddItem_NewOrder_MustExecuteWithSuccess()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);
            _mocker.GetMock<IOrderRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
            //_mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Add New Item Order Draft With Success")]
        [Trait("Category", "Checkout - Order Command Handler")]
        public async Task AddItem_NewItemOrderDraft_MustExecuteWithSuccess()
        {
            //Arrange
            var existingOrderItem = new OrderItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            _order.AddItem(existingOrderItem);
            var orderCommand = new AddOrderItemCommand(_customerId, Guid.NewGuid(), "Produto Teste", 2, 100);

            _mocker.GetMock<IOrderRepository>().Setup(x => x.GetOrderDraftByCustomerId(_customerId)).Returns(Task.FromResult(_order));
            _mocker.GetMock<IOrderRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.AddItem(It.IsAny<OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add Existing Item Order Draft With Success")]
        [Trait("Category", "Checkout - Order Command Handler")]
        public async Task AddItem_ExistingItemOrderDraft_MustExecuteWithSuccess()
        {
            //Arrange
            var existingOrderItem = new OrderItem(_productId, "Produto Xpto", 2, 100);
            _order.AddItem(existingOrderItem);
            var orderCommand = new AddOrderItemCommand(_customerId, _productId, "Produto Teste", 2, 100);

            _mocker.GetMock<IOrderRepository>().Setup(x => x.GetOrderDraftByCustomerId(_customerId)).Returns(Task.FromResult(_order));
            _mocker.GetMock<IOrderRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add Item Command Invalid")]
        [Trait("Category", "Checkout - Order Command Handler")]
        public async Task AddItem_CommandInvalid_MustReturnFalseAndSendNotificationEvent()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            //Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}
