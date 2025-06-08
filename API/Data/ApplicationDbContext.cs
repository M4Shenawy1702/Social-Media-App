using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>(e =>
        {
            e.HasKey(e => new { e.RoleId, e.UserId });
            e.ToTable("UserRoles");
        });

        builder.Ignore<IdentityUserClaim<string>>();
        builder.Ignore<IdentityUserToken<string>>();
        builder.Ignore<IdentityUserLogin<string>>();
        builder.Ignore<IdentityRoleClaim<string>>();

        builder.Entity<UserRelationship>()
            .HasOne(ur => ur.Initiator)
            .WithMany(u => u.RelationshipsInitiated)
            .HasForeignKey(ur => ur.InitiatorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<UserRelationship>()
            .HasOne(ur => ur.Receiver)
            .WithMany(u => u.RelationshipsReceived)
            .HasForeignKey(ur => ur.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Chat>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Chat)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent cascade delete for PostComment → User
        builder.Entity<PostComment>()
            .HasOne(pc => pc.User)
            .WithMany()
            .HasForeignKey(pc => pc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Allow cascade delete for Post → PostComment (optional and safe)
        builder.Entity<PostComment>()
            .HasOne(pc => pc.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(pc => pc.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // You can keep cascade delete for Post → Author if needed,
        // but beware if Post has other cascades (e.g., Media, Likes)
        builder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<UserRelationship> UserRelationships { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<EmailVerification> EmailVerifications { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<Like> Likes { get; set; }
}
