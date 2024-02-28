using System;
using System.Threading.Tasks;

namespace milescarrental.Infrastructure.Processing
{
    public interface ICommandsDispatcher
    {
        Task DispatchCommandAsync(Guid id);
    }
}
