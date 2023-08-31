using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NoticiasAPI.Model.Brevo
{
	public class Pessoa
	{
		public Pessoa()
		{

		}

		public Pessoa(string email, string name)
		{
			Email = email;
			Name = name;
		}

		[JsonPropertyName("email")]
		[Required]
		public string Email { get; set; }
		[JsonPropertyName("name")]
		[Required]
		public string Name { get; set; }
	}
}
