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
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.DateOfBirth)));

            // Bidirectional mapping for UserAddress
            CreateMap<UserAddress, UserAddressDto>()
                .ReverseMap();
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}