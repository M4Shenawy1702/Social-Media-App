using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PostNotFoundException()
    : NotFoundException($"Post was not found");
}