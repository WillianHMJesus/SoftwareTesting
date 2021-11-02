using Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Checkout.Data
{
    public class CheckoutContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public CheckoutContext(DbContextOptions<CheckoutContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;

            if (success)
            {
                await _mediator.PublishEvents(this);
            }

            return success;
        }
    }
}
