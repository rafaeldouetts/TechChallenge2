using NoticiasAPI.Model.Brevo;

namespace NoticiasAPI.Service
{
	public interface IEmailService
	{
		void ConfirmarEmail(ToModel to, string link, string idUsuario);
	}
}
