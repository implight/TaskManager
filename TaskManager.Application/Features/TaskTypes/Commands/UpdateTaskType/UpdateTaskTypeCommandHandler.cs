using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;

namespace TaskManager.Application.Features.TaskTypes.Commands.UpdateTaskType
{
    public class UpdateTaskTypeCommandHandler : IRequestHandler<UpdateTaskTypeCommand, Result>
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateTaskTypeCommandHandler> _logger;

        public UpdateTaskTypeCommandHandler(
            ITaskTypeRepository taskTypeRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<UpdateTaskTypeCommandHandler> logger)
        {
            _taskTypeRepository = taskTypeRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result> Handle(UpdateTaskTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _taskTypeRepository.AnyAsync(x => x.Id != request.Id && x.Name == request.Name, cancellationToken);

                if (exists)
                    return Result.Failure("Task type with this name already exists.");

                var taskType = await _taskTypeRepository.GetByIdAsync(request.Id, cancellationToken);

                if (taskType == null)
                    return Result.Failure("Task type not found");

                taskType.UpdateName(request.Name);

                _taskTypeRepository.Update(taskType);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(taskType);

                _logger.LogInformation("Task type updated: {TaskTypeId}", taskType.Id);

                return Result.Success(taskType.Id);
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed for task type update");
                return Result.Failure<Guid>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renaming task type");
                return Result.Failure<Guid>("An error occurred while updating the task type");
            }
        }
    }
}
