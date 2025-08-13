using System.ComponentModel.DataAnnotations;

namespace myBookSolution.Domain.Models;

public class BorrowingModel
{
    public Int64 Id { get; set; }

    [Required]
    public Int64 UserId { get; set; }

    [Required]
    public Int64 BookId { get; set; }

    public Int64 CuratorId { get; set; }

    [Required]
    public DateTime BorrowingDate { get; set; } = DateTime.Today;

    [Required]
    public bool Status { get; set; } = false; //status = 0 - nao devolvido / status = 1 - devolvido

    public UserModel User { get; set; } = default!;

    public BookModel Book { get; set; } = default!;

    public CuratorModel Curator { get; set; } = default!;
}
