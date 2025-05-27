using AutoMapper;
using com_in.server.DTO;
using com_in.server.Models;

namespace com_in.server.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>();
           
            //CreateMap<Student, StudentDto>()
            //.ForMember(dest => dest.Course,
            //               opt => opt.MapFrom(src => src.course.Name));
                




        }
        
    }
}
