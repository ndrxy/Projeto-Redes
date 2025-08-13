namespace myBookSolution.Domain.Models;
    public class RefreshTokenUserModel
    {
        public long Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public required string Value { get; set; } = string.Empty;
        public required long UserId { get; set; }
        public UserModel User { get; set; } = default!;

    }