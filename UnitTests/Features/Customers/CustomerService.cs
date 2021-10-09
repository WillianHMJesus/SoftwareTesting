using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Features.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;

        public CustomerService(ICustomerRepository customerRepository, 
            IMediator mediator)
        {
            _customerRepository = customerRepository;
            _mediator = mediator;
        }

        public void Add(Customer customer)
        {
            if (!customer.IsValid())
                return;

            _customerRepository.Add(customer);
            _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Olá", "Bem vindo!"));
        }

        public IEnumerable<Customer> GetAllAssets()
        {
            return _customerRepository.GetAll().Where(x => x.Active);
        }

        public void Inactivate(Customer customer)
        {
            if (!customer.IsValid())
                return;

            customer.Inactivate();
            _customerRepository.Update(customer);
            _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Até breve", "Até mais tarde!"));
        }

        public void Remove(Customer customer)
        {
            _customerRepository.Remove(customer.Id);
            _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Adeus", "Tenha uma boa jornada!"));
        }

        public void Update(Customer customer)
        {
            if (!customer.IsValid())
                return;

            _customerRepository.Update(customer);
            _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Mudanças", "Dê uma olhada!"));
        }

        public void Dispose()
        {
            _customerRepository.Dispose();
        }
    }
}
