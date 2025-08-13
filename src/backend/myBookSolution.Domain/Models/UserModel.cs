using System.ComponentModel.DataAnnotations;

namespace myBookSolution.Domain.Models;

public class UserModel
{
    [Required]
    public Int64 Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Password {  get; set; } = string.Empty;

    [Required]
    [MaxLength(11)]
    public string Cpf { set; get; } = string.Empty;

    public Guid UserIdentifier { get; set; }
}
