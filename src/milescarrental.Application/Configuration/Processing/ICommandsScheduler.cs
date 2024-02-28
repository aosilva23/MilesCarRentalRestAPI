using System.Threading.Tasks;
using MediatR;

namespace milescarrental.Application.Configuration.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(IRequest command);
    }
}