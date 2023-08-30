using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Identity.Models
{
	public class Usuario
	{
		public Usuario(string nome, string email, Guid id)
		{
			Id = id;
			Nome = nome;
			Email = email;
		}

		public Guid Id { get; set; }
		public string Nome { get; set; }
		public string Email { get; set; }
	}
}
