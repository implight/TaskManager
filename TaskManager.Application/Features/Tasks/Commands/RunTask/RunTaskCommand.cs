using MediatR;
using TaskManager.Application.Common.Results;

namespace TaskManager.Application.Features.Tasks.Commands.RunTask
{
    public class RunTaskCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
