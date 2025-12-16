using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace TaskManager.WebAPI.Filters
{
    public class AntiInjectionFilter : IActionFilter
    {
        private static readonly Regex SqlInjectionRegex = new(
            @"(\b(SELECT|INSERT|UPDATE|DELETE|DROP|UNION|EXEC|ALTER|CREATE|TRUNCATE)\b|\-\-|;|\/\*)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex JsInjectionRegex = new(
            @"(<script|javascript:|on\w+\s*=)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg is string stringValue)
                {
                    if (ContainsSqlInjection(stringValue))
                    {
                        context.Result = new BadRequestObjectResult(
                            new ProblemDetails
                            {
                                Title = "Invalid input",
                                Detail = "Potential SQL injection detected",
                                Status = StatusCodes.Status400BadRequest,
                                Instance = context.HttpContext.Request.Path
                            });
                        return;
                    }

                    if (ContainsJsInjection(stringValue))
                    {
                        context.Result = new BadRequestObjectResult(
                            new ProblemDetails
                            {
                                Title = "Invalid input",
                                Detail = "Potential JavaScript injection detected",
                                Status = StatusCodes.Status400BadRequest,
                                Instance = context.HttpContext.Request.Path
                            });
                        return;
                    }
                }
                else if (arg != null)
                {
                    CheckObjectForInjection(arg, context);
                    if (context.Result != null) return;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private bool ContainsSqlInjection(string input)
        {
            return SqlInjectionRegex.IsMatch(input);
        }

        private bool ContainsJsInjection(string input)
        {
            return JsInjectionRegex.IsMatch(input);
        }

        private void CheckObjectForInjection(object obj, ActionExecutingContext context)
        {
            var properties = obj.GetType().GetProperties()
                .Where(p => p.CanRead && !p.GetIndexParameters().Any());

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);

                if (value is string stringValue)
                {
                    if (ContainsSqlInjection(stringValue) || ContainsJsInjection(stringValue))
                    {
                        context.Result = new BadRequestObjectResult(
                            new ProblemDetails
                            {
                                Title = "Invalid input",
                                Detail = $"Potential injection detected in property '{property.Name}'",
                                Status = StatusCodes.Status400BadRequest,
                                Instance = context.HttpContext.Request.Path
                            });
                        return;
                    }
                }
                else if (value != null && !value.GetType().IsPrimitive)
                {
                    CheckObjectForInjection(value, context);
                    if (context.Result != null) return;
                }
            }
        }
    }
}
