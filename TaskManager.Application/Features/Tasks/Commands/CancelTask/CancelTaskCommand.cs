using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.Tasks.Commands.CancelTask
{
    public class CancelTaskCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}