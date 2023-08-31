namespace NoticiasAPI.Model
{
	public class ResultTokenPasswordModel
	{
		public ResultTokenPasswordModel(string token, string idUsuario)
		{
			Token = token;
			IdUsuario = idUsuario;
		}

		public string Token { get; set; }
		public string IdUsuario { get; set; }
	}
}
