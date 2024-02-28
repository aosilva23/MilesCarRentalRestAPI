using System.Threading;
using System.Threading.Tasks;
using milescarrental.Domain.SeedWork;
using milescarrental.Infrastructure.Database;
using milescarrental.Infrastructure.Processing;

namespace milescarrental.Infrastructure.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrdersContext _ordersContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(
            OrdersContext ordersContext, 
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            this._ordersContext = ordersContext;
            this._domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this._domainEventsDispatcher.DispatchEventsAsync();
            return await this._ordersContext.SaveChangesAsync(cancellationToken);
        }
    }
}