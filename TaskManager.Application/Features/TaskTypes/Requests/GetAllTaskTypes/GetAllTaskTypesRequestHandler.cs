using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data.Repositories;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Features.TaskTypes.Requests.GetAllTaskTypes
{
    public class GetAllTaskTypesRequestHandler : IRequestHandler<GetAllTaskTypesRequest, Result<TaskTypeDto[]>>
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTaskTypesRequestHandler> _logger;

        public GetAllTaskTypesRequestHandler(
            ITaskTypeRepository taskTypeRepository,
            IMapper mapper,
            ILogger<GetAllTaskTypesRequestHandler> logger)
        {
            _taskTypeRepository = taskTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TaskTypeDto[]>> Handle(GetAllTaskTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var taskTypes = await _taskTypeRepository.GetAllAsync(x => true, cancellationToken);

                return _mapper.Map<TaskTypeDto[]>(taskTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all task types");
                return Result.Failure<TaskTypeDto[]>("An error occurred while retrieving all task types");
            }
        }
    }
}
