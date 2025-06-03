using System;

namespace API.Entities
{
    public class UserRelationship
    {
        public int Id { get; set; }
        public string InitiatorId { get; set; } = null!;
        public ApplicationUser Initiator { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public ApplicationUser Receiver { get; set; } = null!;
        public RelationshipStatus Status { get; set; } = RelationshipStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum RelationshipStatus
    {
        Pending,
        Accepted,
        Blocked,
        Declined,
        None,
        Self
    }
}
