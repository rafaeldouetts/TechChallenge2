using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Identity.Models;

public class CreateUserModel
{
    [Required(ErrorMessage = "Usuário é obrigatório!")]
    public string? UserName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email é obrigatório!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório!")]
    public string? Password { get; set; }

}