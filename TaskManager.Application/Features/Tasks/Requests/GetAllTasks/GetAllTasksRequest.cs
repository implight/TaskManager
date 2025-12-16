using MediatR;
using TaskManager.Application.Common.Results;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Features.Tasks.Requests.GetAllTasks
{
    public class GetAllTasksRequest : IRequest<Result<TaskDto[]>>
    {
    }
}
