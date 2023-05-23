using FunWithEmail.Common;

namespace FunWithEmail.WebApp.Services; 

public static class SmtpRelayExtensions {
	public static WebApplicationBuilder AddSmtpServices(this WebApplicationBuilder builder) {
		var smtpServers = new Dictionary<string, SmtpSettings>();
		builder.Configuration.Bind("Smtp", smtpServers);
		foreach (var server in smtpServers) {
			Console.WriteLine("Creating SMTP relay worker for " + server.Key);
			builder.Services.AddSingleton<IHostedService>(provider => {
				var logger = provider.GetService<ILogger<MailSender>>();
				var queue = provider.GetService<MailQueue>();
				var renderer = provider.GetService<MailRenderer>();
				var tracker = provider.GetService<StatusTracker>();
				var sender = new MailSender(queue!, server.Key, server.Value, renderer!, tracker!, logger!);
				return sender;
			});
		}

		return builder;
	}
}
