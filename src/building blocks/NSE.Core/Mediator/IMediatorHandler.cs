using FluentValidation.Results;
using NSE.Core.Messages;
using System.Threading.Tasks;

namespace NSE.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T ev) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T: Command;
    }
}
