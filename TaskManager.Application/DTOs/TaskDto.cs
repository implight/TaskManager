namespace TaskManager.Application.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public String Status { get; set; }
        public DateTime? StatusAt { get; set; }
        public TaskTypeDto TaskType { get; set; } = null!;
    }
}
