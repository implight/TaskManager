using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Results;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;

namespace TaskManager.Application.Features.Tasks.Commands.RunTask
{
    public class RunTaskCommandHandler : IRequestHandler<RunTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<RunTaskCommandHandler> _logger;

        public RunTaskCommandHandler(
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ILogger<RunTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result> Handle(RunTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

                if (task == null)
                    return Result.Failure("Task not found");

                task.Run();

                _taskRepository.Update(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _mediator.PublishEventsAndClear(task);

                _logger.LogInformation("Task started: {TaskId}", task.Id);

                return Result.Success();
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                _logger.LogWarning(ex, "Cannot run task: {TaskId}", request.Id);
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running task: {TaskId}", request.Id);
                return Result.Failure("An error occurred while running the task");
            }
        }
    }
}
