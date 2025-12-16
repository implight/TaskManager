using MediatR;
using TaskManager.Application.Common.Results;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Features.Tasks.Requests.GetTaskById
{
    public class GetTaskByIdRequest : IRequest<Result<TaskDto?>>
    {
        public Guid Id { get; set; }
    }
}
