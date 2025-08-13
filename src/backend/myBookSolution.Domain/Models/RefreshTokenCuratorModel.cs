namespace myBookSolution.Domain.Models;

public class RefreshTokenCuratorModel
{
    public long Id { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public required string Value { get; set; } = string.Empty;
    public required long CuratorId { get; set; }
    public CuratorModel Curator { get; set; } = default!;
}
