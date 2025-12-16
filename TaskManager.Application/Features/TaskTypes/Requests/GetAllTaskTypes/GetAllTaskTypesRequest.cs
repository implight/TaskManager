using MediatR;
using TaskManager.Application.Common.Results;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Features.TaskTypes.Requests.GetAllTaskTypes
{
    public class GetAllTaskTypesRequest : IRequest<Result<TaskTypeDto[]>>
    {
    }
}
