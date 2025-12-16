using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid TaskTypeId { get; set; }
    }
}
