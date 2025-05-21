using API.Entities;

namespace API.Specifications
{
    internal class SentFriendRequestsSpecification : BaseSpecifications<UserRelationship>
    {
        public SentFriendRequestsSpecification(string initiatorId)
            : base(r => r.InitiatorId == initiatorId && r.Status == RelationshipStatus.Pending)
        {
            AddInclude(r => r.Receiver);
        }
    }
}
