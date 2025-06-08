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
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.AuthorImageUrl, opt => opt.MapFrom(src => src.Author.ProfilePictureUrl));

            CreateMap<Post, LIkedPostDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.AuthorImageUrl, opt => opt.MapFrom(src => src.Author.ProfilePictureUrl));

        }
    }
}
