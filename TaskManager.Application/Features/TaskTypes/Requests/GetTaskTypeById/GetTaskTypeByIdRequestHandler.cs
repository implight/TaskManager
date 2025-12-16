using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data.Repositories;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Features.TaskTypes.Requests.GetTaskTypeById
{
    public class GetTaskTypeByIdRequestHandler : IRequestHandler<GetTaskTypeByIdRequest, Result<TaskTypeDto?>>
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTaskTypeByIdRequestHandler> _logger;

        public GetTaskTypeByIdRequestHandler(
            ITaskTypeRepository taskTypeRepository,
            IMapper mapper,
            ILogger<GetTaskTypeByIdRequestHandler> logger)
        {
            _taskTypeRepository = taskTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TaskTypeDto?>> Handle(GetTaskTypeByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskTypeRepository.GetByIdAsync(request.Id, cancellationToken);

                if (task == null)
                    return Result.Failure<TaskTypeDto?>("Task type not found");

                return _mapper.Map<TaskTypeDto>(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting task type by ID: {TaskId}", request.Id);
                return Result.Failure<TaskTypeDto?>("An error occurred while retrieving the task type");
            }
        }
    }
}
