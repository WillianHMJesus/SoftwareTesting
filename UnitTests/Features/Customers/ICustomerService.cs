using System;
using System.Collections.Generic;

namespace Features.Customers
{
    public interface ICustomerService : IDisposable
    {
        IEnumerable<Customer> GetAllAssets();
        void Add(Customer customer);
        void Update(Customer customer);
        void Remove(Customer customer);
        void Inactivate(Customer customer);
    }
}
