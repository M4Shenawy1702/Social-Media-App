using API.Entities;

namespace API.Specifications
{
internal class ReceivedFriendRequestsSpecification : BaseSpecifications<UserRelationship>
    {
        public ReceivedFriendRequestsSpecification(string receiverId)
            : base(r => r.ReceiverId == receiverId && r.Status == RelationshipStatus.Pending)
        {
            AddInclude(r => r.Initiator);
        }
    }
}
