using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.IService
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string body);
    }
}