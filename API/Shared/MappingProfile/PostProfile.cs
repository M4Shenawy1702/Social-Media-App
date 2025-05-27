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
                // Assuming Media and Comments have their own mappings registered
                .ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.Media))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName)) 
                .ForMember(dest => dest.AuthorImageUrl, opt => opt.MapFrom(src => src.Author.ProfilePictureUrl))  
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));

            
            CreateMap<PostMedia, PostMediaDetailsDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<PostComment, PostCommentDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.AuthorName)); 

            CreateMap<CreatePostDto, Post>()
                .ForMember(dest => dest.Media, opt => opt.Ignore());


        }
    }
}
