using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;
using TaskManager.Application.Common.Extensions;

namespace TaskManager.Application.Features.Tasks.Commands.CancelTask
{
    public class CancelTaskCommandHandler : IRequestHandler<CancelTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<CancelTaskCommandHandler> _logger;

        public CancelTaskCommandHandler(
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<CancelTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result> Handle(CancelTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

                if (task == null)
                    return Result.Failure("Task not found");

                task.Cancel();

                _taskRepository.Update(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(task);

                _logger.LogInformation("Task cancelled: {TaskId}", task.Id);

                return Result.Success();
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Cannot cancel task: {TaskId}", request.Id);
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling task: {TaskId}", request.Id);
                return Result.Failure("An error occurred while cancelling the task");
            }
        }
    }
}
