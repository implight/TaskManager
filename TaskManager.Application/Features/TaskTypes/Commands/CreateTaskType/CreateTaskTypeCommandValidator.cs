using FluentValidation;

namespace TaskManager.Application.Features.TaskTypes.Commands.CreateTaskType
{
    public class CreateTaskTypeCommandValidator : AbstractValidator<CreateTaskTypeCommand>
    {
        public CreateTaskTypeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Task type name is required")
                .MaximumLength(50).WithMessage("Task type name must not exceed 50 characters")
                .MinimumLength(2).WithMessage("Task type name must be at least 2 characters");
        }
    }
}
