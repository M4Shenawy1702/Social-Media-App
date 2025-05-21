using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
         public string ReceiverId { get; set; } = null!;
        public IList<Message> Messages { get; set; } = [];
    }
}