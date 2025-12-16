using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.TaskTypes.Commands.CreateTaskType
{
    public class CreateTaskTypeCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
