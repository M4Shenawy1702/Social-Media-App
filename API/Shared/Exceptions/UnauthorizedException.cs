using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public sealed class UnauthorizedException(string message = "Invalid Credintials")
        : Exception(message);
}
