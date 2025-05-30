using API.Entities;
using API.Shared.Dtos.CommentDtos;
using API.Shared.Dtos.PostDtos;
using AutoMapper;

namespace API.Helpers
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<PostComment, CommentDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.DisplayName))
                .ForMember(dest => dest.AuthorImageUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.UserId));
            CreateMap<CreateCommentDto, PostComment>();
            CreateMap<UpdateCommentDto, PostComment>();
             CreateMap<PostComment, PostCommentDto>(); 
        }
    }
}
