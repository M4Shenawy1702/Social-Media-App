using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.Shared.Dtos.AuthenticationDtos
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public required string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public required string Bio { get; set; }

        [Phone]
        public required string PhoneNumber { get; set; }

        public required IFormFile ProfilePicture{ get; set; }
        public required IFormFile CoverPhoto{ get; set; }


        //Address

        public string City { get; set; }   = default!;
        public string Street { get; set; }  = default!;
        public string Country { get; set; } = default!;
    }
}