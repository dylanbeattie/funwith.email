using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

IConfiguration config = new ConfigurationBuilder()
	.AddJsonFile("appSettings.json")
	.AddEnvironmentVariables()
	.AddUserSecrets(typeof(Program).Assembly)
	.Build();

var smtpServers = config.GetSection("Smtp").Get<SmtpSettings[]>();
var recipients = new[] { "dylan@fm.funwith.email" };
foreach (var server in smtpServers) {
	var client = server.CreateClient();
	foreach (var rcpt in recipients) {
		client.Send("dylan@dylanbeattie.net", rcpt, $"Test message to {rcpt}", $"Test email to {rcpt}");
		Console.WriteLine($"Sent to {rcpt} via {server}");
	}
}
Console.WriteLine("Done! Press any key to exit.");
Console.ReadKey(true);
