using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Dtos
{
    public class MessageDetailsDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string SenderId { get; set; } = null!;
    }
}