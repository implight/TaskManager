using FluentValidation;

namespace TaskManager.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("Task name must not exceed 100 characters")
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("Task name must be at least 3 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description must not exceed 1000 characters");
        }
    }
}
