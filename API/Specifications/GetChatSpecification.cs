using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    internal class GetChatSpecification : BaseSpecifications<Chat>
    {
        public GetChatSpecification(string user1, string user2)
            : base(c =>
                (c.SenderId == user1 && c.ReceiverId == user2) ||
                (c.SenderId == user2 && c.ReceiverId == user1))
        {
            AddInclude(c => c.Messages);
        }
    }
}
