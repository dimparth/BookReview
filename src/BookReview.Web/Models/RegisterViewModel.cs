using System.ComponentModel.DataAnnotations;

namespace BookReview.Web.Models;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; } = default!;
    [Required, EmailAddress]
    public string Email { get; set; } = default!;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = default!;
    [Required, DataType(DataType.Password), Compare("Password")]
    public string ConfirmPassword { get; set; } = default!;
}
