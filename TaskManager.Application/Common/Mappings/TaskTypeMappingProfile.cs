using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Common.Mappings
{
    internal class TaskTypeMappingProfile : Profile
    {
        public TaskTypeMappingProfile()
        {
            CreateMap<TaskType, TaskTypeDto>();
        }
    }
}
