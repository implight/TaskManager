using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.TaskTypes.Commands.DeleteTaskType
{
    public class DeleteTaskTypeCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
