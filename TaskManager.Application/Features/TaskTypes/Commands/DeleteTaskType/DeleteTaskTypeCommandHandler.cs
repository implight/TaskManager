using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;

namespace TaskManager.Application.Features.TaskTypes.Commands.DeleteTaskType
{
    public class DeleteTaskTypeCommandHandler : IRequestHandler<DeleteTaskTypeCommand, Result>
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteTaskTypeCommandHandler> _logger;

        public DeleteTaskTypeCommandHandler(
            ITaskTypeRepository taskTypeRepository,
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<DeleteTaskTypeCommandHandler> logger)
        {
            _taskTypeRepository = taskTypeRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result> Handle(
            DeleteTaskTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var taskType = await _taskTypeRepository.GetByIdAsync(request.Id, cancellationToken);

                if (taskType == null)
                    return Result.Failure("Task type not found");

                var taskAny = await _taskRepository.AnyAsync(x => x.TaskTypeId == request.Id, cancellationToken);

                if (taskAny)
                    return Result.Failure("Cannot delete task type with tasks.");

                taskType.Delete();

                _taskTypeRepository.Delete(taskType);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(taskType);

                _logger.LogInformation("Task type deleted: {TaskTypeId}", taskType.Id);

                return Result.Success();
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Cannot delete task type: {TaskTypeId}", request.Id);
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task type: {TaskTypeId}", request.Id);
                return Result.Failure("An error occurred while deleting the task type");
            }
        }
    }
}
