using System.ComponentModel.DataAnnotations;
using API.Entities;
using API.Shared.Dtos.UserDtos;

public class UserUpdateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string DisplayName { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public  string Bio { get; set; } = null!;
        [Phone]
        public  string PhoneNumber { get; set; } = null!;
        public IFormFile ProfilePicture{ get; set; }
        public IFormFile CoverPhoto{ get; set; }
        //Address
        public string City { get; set; }   = default!;
        public string Street { get; set; }  = default!;
        public string Country { get; set; } = default!;
    }