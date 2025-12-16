using FluentValidation;

namespace TaskManager.Application.Features.TaskTypes.Commands.UpdateTaskType
{
    public class UpdateTaskTypeCommandValidator : AbstractValidator<UpdateTaskTypeCommand>
    {
        public UpdateTaskTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Task type name is required")
                .MaximumLength(50).WithMessage("Task type name must not exceed 50 characters")
                .MinimumLength(2).WithMessage("Task type name must be at least 2 characters");
        }
    }
}
