using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    public class ChechUserRelationshipExistsSpesification
    : BaseSpecifications<UserRelationship>
    {
        public ChechUserRelationshipExistsSpesification(string initiatorId, string receiverId)
    : base(r =>
    (r.InitiatorId == initiatorId && r.ReceiverId == receiverId) || (r.InitiatorId == receiverId && r.ReceiverId == initiatorId))
        {
            AddInclude(r => r.Initiator);
            AddInclude(r => r.Receiver);
        }
    }
}