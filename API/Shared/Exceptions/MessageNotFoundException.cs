using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class MessageNotFoundException()
    : NotFoundException("Message was not found");
}