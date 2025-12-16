using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.Tasks.Commands.PauseTask
{
    public class PauseTaskCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
