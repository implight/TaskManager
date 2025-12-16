using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.Tasks.Commands.CompleteTask
{
    public class CompleteTaskCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
