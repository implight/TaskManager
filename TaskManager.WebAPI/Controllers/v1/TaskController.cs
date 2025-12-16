using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Results;
using TaskManager.Application.DTOs;
using TaskManager.Application.Features.Tasks.Commands.CancelTask;
using TaskManager.Application.Features.Tasks.Commands.CompleteTask;
using TaskManager.Application.Features.Tasks.Commands.CreateTask;
using TaskManager.Application.Features.Tasks.Commands.DeleteTask;
using TaskManager.Application.Features.Tasks.Commands.PauseTask;
using TaskManager.Application.Features.Tasks.Commands.RunTask;
using TaskManager.Application.Features.Tasks.Commands.UpdateTask;
using TaskManager.Application.Features.Tasks.Requests.GetAllTasks;
using TaskManager.Application.Features.Tasks.Requests.GetTaskById;
using TaskManager.WebAPI.Filters;

namespace TaskManager.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    //[Authorize]
    [ServiceFilter(typeof(AntiInjectionFilter))]
    [ServiceFilter(typeof(ValidateModelAttribute))]
    public class TaskController : ApiControllerBase
    {
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetTaskByIdRequest { Id = id });

            return result.IsSuccess ? Ok(result.Value) : NotFoundProblem(result.Error);
        }

        [HttpGet]
        [ProducesResponseType(typeof(TaskDto[]), StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskDto[]>> GetTasks([FromQuery] GetAllTasksRequest request)
        {
            var result = await Mediator.Send(request);

            return result.IsSuccess ? Ok(result.Value) : BadRequestProblem(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateTask([FromBody] CreateTaskCommand command)
        {
            var result = await Mediator.Send(command);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
                : BadRequestProblem(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        {
            var result = await Mediator.Send(command);

            return result.IsSuccess
                ? NoContent()
                : result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? NotFoundProblem(result.Error)
                    : BadRequestProblem(result.Error);
        }

        [HttpPost("{id:guid}/run")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RunTask(Guid id)
        {
            return HandleResult(await Mediator.Send(new RunTaskCommand { Id = id }));
        }

        [HttpPost("{id:guid}/pause")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PauseTask(Guid id)
        {
            return HandleResult(await Mediator.Send(new PauseTaskCommand { Id = id }));

        }

        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelTask(Guid id)
        {
            return HandleResult(await Mediator.Send(new CancelTaskCommand { Id = id }));
        }

        [HttpPost("{id:guid}/complete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompleteTask(Guid id)
        {
            return HandleResult(await Mediator.Send(new CompleteTaskCommand { Id = id }));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var result = await Mediator.Send(new DeleteTaskCommand { Id = id });

            return result.IsSuccess ? NoContent() : NotFoundProblem(result.Error);
        }

        private IActionResult HandleResult(Result result)
        {
            return result.IsSuccess ? NoContent() : BadRequestProblem(result.Error);
        }
    }
}
