using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}