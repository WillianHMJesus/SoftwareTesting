using Bogus;
using Bogus.DataSets;
using Features.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Features.Tests.HumanData
{
    [CollectionDefinition(nameof(CustomerCollection))]
    public class CustomerCollection : ICollectionFixture<CustomerBogusFixture>
    { }

    public class CustomerBogusFixture : IDisposable
    {
        public Customer GenerateValidCustomer()
        {
            return GenerateCustomers(1, true).FirstOrDefault();
        }

        public Customer GenerateInvalidCustomer()
        {
            var gender = new Faker().PickRandom<Name.Gender>();
            var customer = new Faker<Customer>("pt_BR")
                .CustomInstantiator(x => new Customer(
                    Guid.NewGuid(),
                    x.Name.FirstName(gender),
                    x.Name.LastName(gender),
                    x.Date.Past(1, DateTime.Now.AddYears(1)),
                    "",
                    DateTime.Now,
                    false));

            return customer;
        }

        public IEnumerable<Customer> GenerateVariedCustomers()
        {
            var customers = new List<Customer>();
            customers.AddRange(GenerateCustomers(50, true));
            customers.AddRange(GenerateCustomers(50, false));

            return customers;
        }

        public IEnumerable<Customer> GenerateCustomers(int quantity, bool active)
        {
            var gender = new Faker().PickRandom<Name.Gender>();
            var customer = new Faker<Customer>("pt_BR")
                .CustomInstantiator(x => new Customer(
                    Guid.NewGuid(),
                    x.Name.FirstName(gender),
                    x.Name.LastName(gender),
                    x.Date.Past(50, DateTime.Now.AddYears(-18)),
                    "",
                    DateTime.Now,
                    active))
                .RuleFor(x => x.Email, (r, x) => r.Internet.Email(x.Name.ToLower(), x.Surname.ToLower()));

            return customer.Generate(quantity);
        }

        public void Dispose()
        {

        }
    }
}
