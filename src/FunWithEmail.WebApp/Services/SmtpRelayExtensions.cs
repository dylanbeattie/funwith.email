using DnsClient;
using FunWithEmail.Common;

namespace FunWithEmail.WebApp.Services;

public static class SmtpRelayExtensions {
	public static WebApplicationBuilder AddSmtpServices(this WebApplicationBuilder builder) {
		var smtpServers = new Dictionary<string, SmtpSettings>();
		if (builder.Environment.IsDevelopment()) {
			smtpServers.Add("papercut", new() {
				Host = "localhost",
				Port = 25
			});
		} else {
			builder.Configuration.Bind("Smtp", smtpServers);
		}

		foreach (var server in smtpServers) {
			Console.WriteLine("Creating SMTP relay worker for " + server.Key);
			builder.Services.AddSingleton<IHostedService>(provider => {
				var logger = provider.GetService<ILogger<MailSender>>();
				var queue = provider.GetService<MailQueue>();
				var renderer = provider.GetService<MailRenderer>();
				var tracker = provider.GetService<StatusTracker>();
				var dns = provider.GetService<ILookupClient>();
				var sender = new MailSender(queue!, server.Key, server.Value, renderer!, tracker!, dns!, logger!);
				return sender;
			});
		}

		return builder;
	}
}
