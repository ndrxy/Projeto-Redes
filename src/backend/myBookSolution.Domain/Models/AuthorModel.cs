using System.ComponentModel.DataAnnotations;

namespace myBookSolution.Domain.Models;

public class AuthorModel
{
    [Required]
    public Int64 Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(400)]
    public string Description { get; set; } = string.Empty;

    public Int64 CuratorId { get; set; }

    public CuratorModel Curator { get; set; }

    public ICollection<BookModel> Books { get; set; } = [];
}
