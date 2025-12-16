using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;

namespace TaskManager.Application.Features.Tasks.Commands.PauseTask
{
    public class PauseTaskCommandHandler : IRequestHandler<PauseTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<PauseTaskCommandHandler> _logger;

        public PauseTaskCommandHandler(
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<PauseTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result> Handle(PauseTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

                if (task == null)
                    return Result.Failure("Task not found");

                task.Pause();

                _taskRepository.Update(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(task);

                _logger.LogInformation("Task paused: {TaskId}", task.Id);

                return Result.Success();
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Cannot pause task: {TaskId}", request.Id);
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pausing task: {TaskId}", request.Id);
                return Result.Failure("An error occurred while pausing the task");
            }
        }
    }
}
