using Api.Models;
using System;
using System.Linq;

namespace Api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void Add(Order order)
        {
            CartMock.Cart.Add(order);
        }

        public Order GetByCustomerId(Guid customerId)
        {
            return CartMock.Cart.FirstOrDefault(x => x.CustomerId == customerId);
        }
    }
}
