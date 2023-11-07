using TechChallenge.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Serilog;
using NoticiasAPI.Model;
using NoticiasAPI.Service;
using NoticiasAPI.Model.Brevo;

namespace TechChallenge.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
	private readonly HttpClient _httpClient;
	private readonly IEmailService _emailService;
	//private readonly RoleManager<IdentityRole> _roleManager;

	public AuthenticateController(
        IConfiguration configuration,
        UserManager<IdentityUser> userManager,
        HttpClient httpClient,
		IEmailService emailService)
    //RoleManager<IdentityRole> roleManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _httpClient = httpClient;
        _emailService = emailService;
        //_roleManager = roleManager;

    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.UserName);

        if(string.IsNullOrEmpty(model.UserName))
			return BadRequest(new ResponseModel { Success = false, Message = "Informe o nome do usuario!" });

		if (userExists is not null)
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel { Success = false, Message = "Usuário já existe!" }
            );

        IdentityUser user = new()
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = model.Email,
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel { Success = false, Message = result.Errors.First().Description }
            );

		string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

		var link = string.Concat("https://localhost:7234/api/authenticate/confirmar-email/", user.Id, "/", code.Replace("/", "%"));

        _emailService.ConfirmarEmail(new ToModel(user.Email, user.UserName), link, user.Id);

		return Ok(new ResponseModel { Message = "Usuário criado com sucesso!" });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password))
        {

            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, user.Id)
            };

            return Ok(new ResponseModel { Data = GetToken(authClaims, model.UserName) });
        }

        return Unauthorized();
    }


    [HttpGet]
    [Route("authenticated")]
    [Authorize]
    public string GetAuthenticated() => $"Usuário autenticado: {User?.Identity?.Name} ";

	[HttpPost]
	[Route("generate-token-reset-password")]
	public async Task<ActionResult> GenerateTokenResetPassword(string email)
	{
		var user = await _userManager.FindByEmailAsync(email);

		if (user == null || user.PasswordHash == null)
			return BadRequest();

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);

		if (string.IsNullOrEmpty(token)) return BadRequest("Náo foi possivel gerar o token.");

		var result = new ResultTokenPasswordModel(token, user.Id);

		return Ok(result);
	}

	[HttpPost]
	[Route("redefinir-senha/{idUsuario}")]
	public async Task<ActionResult> ResetPassword(string idUsuario, ResetPasswordModel model)
	{
		var user = await _userManager.FindByIdAsync(idUsuario);

		if (user == null || user.PasswordHash == null)
			return BadRequest();

		var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Senha);

		if (!result.Succeeded) return BadRequest(result.Errors);

		return Ok();
	}

	[HttpGet]
	[Route("confirmar-email/{idUsuario}/{tokenConfirmacao}")]
	public async Task<IActionResult> ConfirmacaoEmail(string idUsuario, string tokenConfirmacao)
	{
		var user = await _userManager.FindByIdAsync(idUsuario);

		if (user == null)
			return BadRequest();

		await _userManager.ConfirmEmailAsync(user, tokenConfirmacao);

		return Ok();
	}


	private TokenModel GetToken(List<Claim> authClaims, string nome)
    {
        //obtém a chave de assinatura do JWT
        var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        //Monta o TOKEN
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        //Retorna o token + validade
        return new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ValidTo = token.ValidTo,
            Nome = nome
        };

    }
}