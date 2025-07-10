using Microsoft.AspNetCore.Identity;
namespace API.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Profile Information
        public string DisplayName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        // Social Media Specific
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; }

        // Relationships
        public ICollection<UserRelationship> RelationshipsInitiated { get; set; } = new List<UserRelationship>();
        public ICollection<UserRelationship> RelationshipsReceived { get; set; } = new List<UserRelationship>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        //address
        public UserAddress? UserAddress { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}