using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<Guid>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskTypeRepository _taskTypeRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateTaskCommandHandler> _logger;

        public CreateTaskCommandHandler(
            ITaskRepository taskRepository,
            ITaskTypeRepository taskTypeRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<CreateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _taskTypeRepository = taskTypeRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskType = await _taskTypeRepository.GetByIdAsync(request.TaskTypeId, cancellationToken);

                if (taskType == null)
                    return Result.Failure<Guid>("Task type not found or has been deleted");

                var task = Task.Create(request.Name, request.Description, taskType);

                _taskRepository.Add(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(task);

                _logger.LogInformation("Task created with ID: {TaskId}", task.Id);

                return Result.Success(task.Id);
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed for task creation");
                return Result.Failure<Guid>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return Result.Failure<Guid>("An error occurred while creating the task");
            }
        }
    }
}
