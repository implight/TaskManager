using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Exceptions
{
    public class TaskDomainException : DomainException
    {
        public TaskDomainException(string message) : base(message) { }

        public static TaskDomainException InvalidStatusTransition(TaskStatus from, TaskStatus to)
        {
            return new TaskDomainException($"Cannot transition task status from '{from}' to '{to}'");
        }

        public static TaskDomainException CannotModifyFinalState()
        {
            return new TaskDomainException("Cannot modify task in final state (Cancelled or Completed)");
        }
    }
}
