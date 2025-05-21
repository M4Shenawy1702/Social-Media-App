using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class ServiceException : Exception
    {
         public int StatusCode { get; }

        public ServiceException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}