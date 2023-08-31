using NoticiasAPI.Model.Brevo;
using NoticiasAPI.Model.Brevo.Config;
using NoticiasAPI.Model.Brevo.email;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NoticiasAPI.Service
{
	public class BrevoEmailService : IEmailService
	{
		private readonly HttpClient _client;
		private readonly SenderModel _sender;
		private readonly IHttpClientFactory _clientFactory;

		public BrevoEmailService(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
			_sender = new SenderModel("no-reply@techchallenge.com", "techChallenge");
			_client = _clientFactory.CreateClient("Brevo");
		}

		public void ConfirmarEmail(ToModel to, string link, string idUsuario)
		{
			var email = new BoasVindas(link);

			var lista = new List<ToModel>();
			lista.Add(to);

			Enviar(lista, "Boas Vindas!", email.Corpo, idUsuario);
		}

		private async void Enviar(List<ToModel> to, string subject, string htmlContent, string idUsuario)
		{
			try
			{
				var tag = new List<string>();

				tag.Add(idUsuario);

				var email = new SmtpEmailModel(_sender, to, htmlContent, subject, tags: tag);

				var json = JsonSerializer.Serialize<SmtpEmailModel>(email);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var result = await _client.PostAsync("smtp/email", content);
			}
			catch (Exception e)
			{
				new Exception();
			}
		}
	}
}
