using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Enums;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Common.Mappings
{
    internal class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<Task, TaskDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToFriendlyString()));
        }
    }
}
