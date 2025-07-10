using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    public class GetMessageSpecification
    : BaseSpecifications<Message>
    {
        public GetMessageSpecification(int messageId)
        : base(m => m.Id == messageId)
        {
            AddInclude(m => m.Chat);
        }
    }
}