using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data.Repositories;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Features.Tasks.Requests.GetAllTasks
{
    public class GetAllTasksRequestHandler : IRequestHandler<GetAllTasksRequest, Result<TaskDto[]>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTasksRequestHandler> _logger;

        public GetAllTasksRequestHandler(
            ITaskRepository taskRepository,
            IMapper mapper,
            ILogger<GetAllTasksRequestHandler> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TaskDto[]>> Handle(GetAllTasksRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var tasks = await _taskRepository.GetAllAsync(x => true, cancellationToken);

                var taskDtos = _mapper.Map<TaskDto[]>(tasks);

                return Result.Success(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tasks");
                return Result.Failure<TaskDto[]>("An error occurred while retrieving tasks");
            }
        }
    }
}
