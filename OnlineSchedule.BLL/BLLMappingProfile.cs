using AutoMapper;
using CONTRACT.DTO;
using DAL.Entities;

namespace BLL;

public class BLLMappingProfile : Profile
{
    public BLLMappingProfile()
    {
        CreateMap<User, UserInfo>();
        
        CreateMap<Department, DepartmentInfo>();
        
        CreateMap<Teacher, TeacherInfo>()
            .ForCtorParam("DepartmentName", 
                opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : "Unknown"));
                
        CreateMap<Group, GroupInfo>()
            .ForCtorParam("DepartmentName", 
                opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : "Unknown"));
                
        CreateMap<Classroom, ClassroomInfo>();
        
        CreateMap<Discipline, DisciplineInfo>();
        
        CreateMap<ScheduleItem, ScheduleItemInfo>()
            .ForCtorParam("GroupName", 
                opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : "Unknown"))
            .ForCtorParam("TeacherName", 
                opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Name : "Unknown"))
            .ForCtorParam("ClassroomName", 
                opt => opt.MapFrom(src => src.Classroom != null ? src.Classroom.Name : "Unknown"))
            .ForCtorParam("DisciplineName", 
                opt => opt.MapFrom(src => src.Discipline != null ? src.Discipline.Name : "Unknown"))
            .ForCtorParam("WeekType", 
                opt => opt.MapFrom(src => src.WeekType.ToString()));
    }
}
