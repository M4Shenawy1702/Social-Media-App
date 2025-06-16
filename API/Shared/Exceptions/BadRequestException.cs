using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class BadRequestException(List<string> errors)
        : Exception("Validation faild")
    {
        public List<string> Errors { get; } = errors;
    }
}
