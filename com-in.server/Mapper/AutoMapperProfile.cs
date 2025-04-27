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
                .ForMember(d => d.CategoryName,
                            opt => opt.MapFrom(src => src.Category.CategoryName));
            CreateMap<Media, MediaDto>()
                .ForMember(dest => dest.Category,
                            opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.Type,
                            opt => opt.MapFrom(src => src.Type.Name));
                
        }
        
    }
}
