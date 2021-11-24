using Api.Models;
using System;

namespace Api.Repositories
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Order GetByCustomerId(Guid customerId);
    }
}
