using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Infrastructure.Data.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(t => t.StatusAt)
                .HasColumnName("status_at")
                .IsRequired();

            builder.HasOne(t => t.TaskType)
                .WithMany()
                .HasForeignKey(t => t.TaskTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
