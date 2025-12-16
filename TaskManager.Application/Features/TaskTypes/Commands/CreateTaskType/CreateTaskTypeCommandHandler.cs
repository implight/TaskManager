using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Features.TaskTypes.Commands.CreateTaskType
{
    public class CreateTaskTypeCommandHandler : IRequestHandler<CreateTaskTypeCommand, Result<Guid>>
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateTaskTypeCommandHandler> _logger;

        public CreateTaskTypeCommandHandler(
            ITaskTypeRepository taskTypeRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<CreateTaskTypeCommandHandler> logger)
        {
            _taskTypeRepository = taskTypeRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateTaskTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _taskTypeRepository.AnyAsync(x => x.Name == request.Name, cancellationToken);

                if (exists)
                    return Result.Failure<Guid>("Task type with this name already exists.");

                var taskType = TaskType.Create(request.Name);

                _taskTypeRepository.Add(taskType);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(taskType);

                _logger.LogInformation("Task type created: {TaskTypeId}", taskType.Id);

                return Result.Success(taskType.Id);
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed for task type creation");
                return Result.Failure<Guid>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task type");
                return Result.Failure<Guid>("An error occurred while creating the task type");
            }
        }
    }
}
