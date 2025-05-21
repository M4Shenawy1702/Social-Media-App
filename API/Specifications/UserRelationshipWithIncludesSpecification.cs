using API.Entities;
using System;
using System.Linq.Expressions;

namespace API.Specifications
{
    internal class UserRelationshipWithIncludesSpecification : BaseSpecifications<UserRelationship>
    {
        public UserRelationshipWithIncludesSpecification(string initiatorId , string receiverId)
            : base(r =>
            ((r.InitiatorId == initiatorId && r.ReceiverId == receiverId) || (r.InitiatorId == receiverId && r.ReceiverId == initiatorId))
            && (r.Status == RelationshipStatus.Accepted || r.Status == RelationshipStatus.Pending))
        {
        AddInclude(r => r.Initiator);
        AddInclude(r => r.Receiver);
        }
         public UserRelationshipWithIncludesSpecification(string userId)
            : base(r =>
            (r.InitiatorId == userId || r.ReceiverId == userId)
            && r.Status == RelationshipStatus.Accepted)      
        {
        AddInclude(r => r.Initiator);
        AddInclude(r => r.Receiver);
        }
        public UserRelationshipWithIncludesSpecification()
            : base(null)
        {
        AddInclude(r => r.Initiator);
        AddInclude(r => r.Receiver);
        }
    }
}
