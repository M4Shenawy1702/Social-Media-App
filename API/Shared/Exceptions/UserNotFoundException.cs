using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public sealed class UserNotFoundException(string id)
        : NotFoundException($"No User With Id {id} was Found!");
}
