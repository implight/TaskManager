using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Features.TaskTypes.Commands.CreateTaskType;
using TaskManager.Application.Features.TaskTypes.Commands.DeleteTaskType;
using TaskManager.Application.Features.TaskTypes.Commands.UpdateTaskType;
using TaskManager.Application.Features.TaskTypes.Requests.GetAllTaskTypes;
using TaskManager.Application.Features.TaskTypes.Requests.GetTaskTypeById;
using TaskManager.WebAPI.Filters;

namespace TaskManager.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    //[Authorize]
    [ServiceFilter(typeof(AntiInjectionFilter))]
    [ServiceFilter(typeof(ValidateModelAttribute))]
    public class TaskTypeController : ApiControllerBase
    {
        private readonly ILogger<TaskTypeController> _logger;

        public TaskTypeController(ILogger<TaskTypeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskTypeDto>> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetTaskTypeByIdRequest { Id = id });

            return result.IsSuccess ? Ok(result.Value) : NotFoundProblem(result.Error);
        }

        [HttpGet]
        [ProducesResponseType(typeof(TaskTypeDto[]), StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskTypeDto[]>> GetTaskTypes([FromQuery] GetAllTaskTypesRequest request)
        {
            var result = await Mediator.Send(request);

            return result.IsSuccess ? Ok(result.Value) : BadRequestProblem(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateTaskType([FromBody] CreateTaskTypeCommand command)
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
        public async Task<IActionResult> UpdateTaskType([FromBody] UpdateTaskTypeCommand command)
        {
            var result = await Mediator.Send(command);

            return result.IsSuccess
                ? NoContent()
                : result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? NotFoundProblem(result.Error)
                    : BadRequestProblem(result.Error);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTaskType(Guid id)
        {
            var result = await Mediator.Send(new DeleteTaskTypeCommand { Id = id });

            return result.IsSuccess ? NoContent() : NotFoundProblem(result.Error);
        }
    }
}
