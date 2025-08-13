using System.ComponentModel.DataAnnotations;

namespace myBookSolution.Domain.Models;

public class BookModel
{
    [Required]
    public Int64 Id { get; set; }

    [Required] 
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(400)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(17)]
    public string Isbn {  get; set; } = string.Empty;

    public int Quantity { get; set; }

    public AuthorModel Author { get; set; } = default!;

    public CuratorModel Curator { get; set; } = default!;

    public Int64 AuthorId {  get; set; }

    public Int64 CuratorId { get; set; }


    
}
