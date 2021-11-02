using Core.DomainObjects;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Data
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IMediator mediator, CheckoutContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Events)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
