using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Dtos.AuthenticationDtos
{
    public record LoginRequest([EmailAddress][Required]string Email,[Required] string Password);
}
