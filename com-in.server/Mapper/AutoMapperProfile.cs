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
            CreateMap<Article, ArticleDto>()
                .ForMember(d => d.category,
                            opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Media, MediaDto>()
                .ForMember(dest => dest.category,
                            opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Type,
                            opt => opt.MapFrom(src => src.Type.Name));

            CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Course,
                           opt => opt.MapFrom(src => src.course.Name));
                


        }
        
    }
}
