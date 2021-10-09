using Features.Core;

namespace Features.Customers
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer GetByEmail(string email);
    }
}
