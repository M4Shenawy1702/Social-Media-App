using System;
using System.Collections.Generic;
using API.Entities;

namespace API.Shared.Dtos.UserDtos
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CoverPhotoUrl { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOnline { get; set; }
        public UserAddressDto UserAddress { get; set; }
        public Gender Gender { get; set; }
        public int FriendsCount { get; set;}
        public UserDetailsDto() { }
        public UserDetailsDto(
            Guid id,
            string userName,
            string email,
            string displayName,
            string profilePictureUrl,
            string coverPhotoUrl,
            string bio,
            DateTime dateOfBirth,
            DateTime createdAt,
            bool isOnline,
            UserAddressDto address)
        {
            Id = id;
            UserName = userName;
            Email = email;
            DisplayName = displayName;
            ProfilePictureUrl = profilePictureUrl;
            CoverPhotoUrl = coverPhotoUrl;
            Bio = bio;
            DateOfBirth = dateOfBirth;
            CreatedAt = createdAt;
            IsOnline = isOnline;
            UserAddress = address;
        }

        private int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}