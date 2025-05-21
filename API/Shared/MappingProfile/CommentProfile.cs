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
            CreateMap<PostComment, CommentDto>();
            CreateMap<CreateCommentDto, PostComment>();
            CreateMap<UpdateCommentDto, PostComment>();
             CreateMap<PostComment, PostCommentDto>(); 
        }
    }
}
