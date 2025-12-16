
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Data.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
    }
}
