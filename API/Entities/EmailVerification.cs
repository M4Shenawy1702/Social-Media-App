namespace API.Entities
{
    public class EmailVerification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}