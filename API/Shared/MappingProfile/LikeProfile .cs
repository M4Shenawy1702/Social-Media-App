using AutoMapper;
using API.Entities;
using API.Shared.Dtos.LikeDtos;

namespace API.Shared.MappingProfile
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            CreateMap<Like, LikeDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.LikedAt, opt => opt.MapFrom(src => src.LikedAt));
        }
    }
}
