using TaskManager.Application.Data.Repositories;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Repositories
{
    public class TaskTypeRepository : Repository<TaskType>, ITaskTypeRepository
    {
        public TaskTypeRepository(AppDbContext context) : base(context)
        {
        }

        protected override IQueryable<TaskType> Include(IQueryable<TaskType> set) => set;
    }
}
