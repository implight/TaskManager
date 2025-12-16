using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;

namespace TaskManager.Application.Features.Tasks.Commands.CompleteTask
{
    public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<CompleteTaskCommandHandler> _logger;

        public CompleteTaskCommandHandler(
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<CompleteTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

                if (task == null)
                    return Result.Failure("Task not found");

                task.Complete();

                _taskRepository.Update(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(task);

                _logger.LogInformation("Task completed: {TaskId}", task.Id);

                return Result.Success();
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Cannot complete task: {TaskId}", request.Id);
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing task: {TaskId}", request.Id);
                return Result.Failure("An error occurred while completing the task");
            }
        }
    }
}
