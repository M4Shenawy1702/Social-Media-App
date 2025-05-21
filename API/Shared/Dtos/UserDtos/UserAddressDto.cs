using System.ComponentModel.DataAnnotations;

namespace API.Shared.Dtos.UserDtos
{
    public record UserAddressDto(
        [property: Required, MaxLength(100)]
        string City,
        
        [property: Required, MaxLength(200)]
        string Street,
        
        [property: Required, MaxLength(100)]
        string Country,
        
        [property: Required]
        string UserId
    );
}