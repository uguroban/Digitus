using System.ComponentModel.DataAnnotations;

namespace Digitus.Dtos;

public class SignupDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    [Required,EmailAddress]
    public string? Email { get; set; }
    
    public string? Password { get; set; }
    [Required,Compare("Password")]
    public string? PasswordConfirm { get; set; }
}