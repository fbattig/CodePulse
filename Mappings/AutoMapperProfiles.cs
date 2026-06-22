using AutoMapper;
using CodePulse.API.Models.Domains;
using CodePulse.API.Models.DTO;

namespace CodePulse.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryRequestDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UpdateCategoryRequestDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // BlogPost
            CreateMap<BlogPost, BlogPostDto>().ReverseMap();
            CreateMap<CreateBlogPostRequestDto, BlogPost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UpdateBlogPostRequestDto, BlogPost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
