using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.TaskTypes.Commands.UpdateTaskType
{
    public class UpdateTaskTypeCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
