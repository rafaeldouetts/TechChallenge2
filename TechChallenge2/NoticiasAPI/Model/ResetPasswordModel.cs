using System.ComponentModel.DataAnnotations;

namespace NoticiasAPI.Model
{
	public class ResetPasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		public string Senha { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirmar a senha")]
		[Compare(nameof(Senha), ErrorMessage = "A senhe e a confirmação da senha são diferentes")]
		public string ConfirmaSenha { get; set; }
		
		[Required]
		public string Token { get; set; }
	}
}
