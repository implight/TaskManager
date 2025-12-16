using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data.Repositories;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Features.Tasks.Requests.GetTaskById
{
    public class GetTaskByIdRequestHandler : IRequestHandler<GetTaskByIdRequest, Result<TaskDto?>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTaskByIdRequestHandler> _logger;

        public GetTaskByIdRequestHandler(
            ITaskRepository taskRepository,
            IMapper mapper,
            ILogger<GetTaskByIdRequestHandler> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TaskDto?>> Handle(GetTaskByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

                if (task == null)
                    return Result.Failure<TaskDto?>("Task not found");

                return _mapper.Map<TaskDto>(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting task by ID: {TaskId}", request.Id);
                return Result.Failure<TaskDto?>("An error occurred while retrieving the task");
            }
        }
    }
}
