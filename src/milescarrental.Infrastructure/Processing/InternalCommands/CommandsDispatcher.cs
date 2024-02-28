using System;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using milescarrental.Application.Configuration.Processing;

using milescarrental.Infrastructure.Database;

namespace milescarrental.Infrastructure.Processing.InternalCommands
{
    public class CommandsDispatcher : ICommandsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly OrdersContext _ordersContext;

        public CommandsDispatcher(
            IMediator mediator, 
            OrdersContext ordersContext)
        {
            this._mediator = mediator;
            this._ordersContext = ordersContext;
        }

        public async Task DispatchCommandAsync(Guid id)
        {
            var command = await this._ordersContext.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);

            command.ProcessedDate = DateTime.UtcNow;

           
        }
    }
}