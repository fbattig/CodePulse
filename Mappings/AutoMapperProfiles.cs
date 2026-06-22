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
            CreateMap<CreateCategoryRequestDto, Category>();
            CreateMap<UpdateCategoryRequestDto, Category>();

            // BlogPost
            CreateMap<BlogPost, BlogPostDto>().ReverseMap();
            CreateMap<CreateBlogPostRequestDto, BlogPost>();
            CreateMap<UpdateBlogPostRequestDto, BlogPost>();
        }
    }
}
