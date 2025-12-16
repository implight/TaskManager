
using FluentValidation;

namespace TaskManager.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Task name is required")
                .MaximumLength(100).WithMessage("Task name must not exceed 100 characters")
                .MinimumLength(3).WithMessage("Task name must be at least 3 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.TaskTypeId)
                .NotEmpty().WithMessage("Task type is required");
        }
    }
}
