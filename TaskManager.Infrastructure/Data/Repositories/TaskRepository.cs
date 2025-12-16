using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Data.Repositories;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Infrastructure.Data.Repositories
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public static readonly Func<AppDbContext, Guid, CancellationToken, Task<Task?>> GetById =
            EF.CompileAsyncQuery((AppDbContext context, Guid id, CancellationToken ct) =>
                context.Tasks
                    .Include(t => t.TaskType)
                    .SingleOrDefault(t => t.Id == id));

        public TaskRepository(AppDbContext context) : base(context)
        {
        }

        protected override IQueryable<Task> Include(IQueryable<Task> set)
            => set.Include(x => x.TaskType);

        public override Task<Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return GetById(_context, id, cancellationToken);
        }
    }
}
