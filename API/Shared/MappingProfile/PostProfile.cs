using AutoMapper;
using API.Entities;
using API.Shared.Dtos.PostDtos;

namespace API.Shared.MappingProfile
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDto>()
                 .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.Media))
                .ReverseMap();

            CreateMap<PostMedia, PostMediaDto>()
                .ReverseMap();

            CreateMap<CreatePostDto, Post>()
                .ForMember(dest => dest.Media, opt => opt.Ignore());

            
        CreateMap<PostMedia, PostMediaDetailsDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));    
        }
    }
}
