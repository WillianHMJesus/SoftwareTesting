using Features.Customers;
using System;
using Xunit;

namespace Features.Tests.Fixtures
{
    public class CustomerFixture : IDisposable
    {
        public Customer GenerateValidCustomer()
        {
            var customer = new Customer(
                Guid.NewGuid(),
                "Willian",
                "Jesus",
                DateTime.Now.AddYears(-30),
                "willian@jesus.com",
                DateTime.Now,
                true);

            return customer;
        }

        public Customer GenerateInvalidCustomer()
        {
            var customer = new Customer(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "willian2jesus.com",
                DateTime.Now,
                true);

            return customer;
        }

        public void Dispose()
        {
            
        }
    }
}
