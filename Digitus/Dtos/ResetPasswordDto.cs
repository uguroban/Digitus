using System.ComponentModel.DataAnnotations;

namespace Digitus.Dtos;

public class ResetPasswordDto
{
    [Required] 
    public string? PasswordResetCode { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required,Compare("Password",ErrorMessage = "Password does not match")]
    public string? PasswordConfirm { get; set; }
}