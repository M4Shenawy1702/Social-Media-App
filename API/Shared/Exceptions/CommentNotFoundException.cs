using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class CommentNotFoundException(int id)
    : Exception($"Comment With id {id} was not found");
}