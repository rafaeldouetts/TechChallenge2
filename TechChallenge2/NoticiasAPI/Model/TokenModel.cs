namespace TechChallenge.Identity.Models;
public class TokenModel
{
    public string? Token { get; set; }
    public DateTime ValidTo { get; set; }
    public string Nome { get; set; }
}
