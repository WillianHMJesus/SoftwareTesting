using Core.Data;
using System;
using System.Threading.Tasks;

namespace Checkout.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
        void AddItem(OrderItem orderItem);
        Task<Order> GetOrderDraftByCustomerId(Guid customerId);
        void Update(Order order);
        void UpdateItem(OrderItem orderItem);
    }
}
