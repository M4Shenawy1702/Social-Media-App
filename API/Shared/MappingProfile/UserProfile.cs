using API.Entities;
using API.Shared.Dtos.UserDtos;
using AutoMapper;
using System;

namespace API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // ApplicationUser to UserResponseDto mapping
            CreateMap<ApplicationUser, UserDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
               .ForMember(dest => dest.FriendsCount, opt => opt.MapFrom(src =>
                src.RelationshipsInitiated.Count(r => r.Status == RelationshipStatus.Accepted) +
                src.RelationshipsReceived.Count(r => r.Status == RelationshipStatus.Accepted)));


            // Bidirectional mapping for UserAddress
            CreateMap<UserAddress, UserAddressDto>()
                .ReverseMap();
        }

        private int CalculateFriendsCount(ApplicationUser user)
        {
            var initiated = user.RelationshipsInitiated
                .Count(r => r.Status == RelationshipStatus.Accepted);

            var received = user.RelationshipsReceived
                .Count(r => r.Status == RelationshipStatus.Accepted);

            return initiated + received;
        }

    }
}