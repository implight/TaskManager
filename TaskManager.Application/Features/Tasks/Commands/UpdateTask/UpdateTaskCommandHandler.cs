using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;

namespace TaskManager.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateTaskCommandHandler> _logger;

        public UpdateTaskCommandHandler(
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<UpdateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

                if (task == null)
                    return Result.Failure("Task not found");

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    task.UpdateName(request.Name);
                }

                task.UpdateDescription(request.Description);

                _taskRepository.Update(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(task);

                _logger.LogInformation("Task updated: {TaskId}", task.Id);

                return Result.Success();
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed for task update");
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task");
                return Result.Failure("An error occurred while updating the task");
            }
        }
    }
}
