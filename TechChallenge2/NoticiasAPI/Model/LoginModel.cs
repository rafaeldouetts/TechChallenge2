using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Identity.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Usuário é obrigatório!")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório!")]
    public string? Password { get; set; }
}
