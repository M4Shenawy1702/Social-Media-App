using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Dtos.AuthenticationDtos
{
    public record UserResponse(
    string UserId,
    string Email,
    string UserName,
    string ProfilePictureUrl,
    string? DisplayName,
    string Token
);
}
