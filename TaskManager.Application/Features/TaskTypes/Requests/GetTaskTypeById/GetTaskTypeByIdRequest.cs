using MediatR;
using TaskManager.Application.Common.Results;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Features.TaskTypes.Requests.GetTaskTypeById
{
    public class GetTaskTypeByIdRequest : IRequest<Result<TaskTypeDto?>>
    {
        public Guid Id { get; set; }
    }
}
