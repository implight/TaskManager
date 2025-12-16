using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.WebAPI.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender? _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected NotFoundObjectResult NotFoundProblem(string detail)
        {
            return NotFound(
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext,
                    statusCode: StatusCodes.Status404NotFound,
                    detail: detail
                ));
        }

        protected BadRequestObjectResult BadRequestProblem(string detail)
        {
            return BadRequest(ProblemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: 400,
                detail: detail));
        }
    }
}
