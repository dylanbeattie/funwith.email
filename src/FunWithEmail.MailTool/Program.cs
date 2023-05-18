using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

IConfiguration config = new ConfigurationBuilder()
	.AddJsonFile("appSettings.json")
	.AddEnvironmentVariables()
	.AddUserSecrets(typeof(Program).Assembly)
	.Build();

var from = "hello@funwith.email";
var smtpServers = config.GetSection("Smtp").Get<SmtpSettings[]>();
var recipients = new[] { "dylan@fm.funwith.email", "dylan@dylanbeattie.net", "dylan.beattie@gmail.com" };
foreach (var server in smtpServers) {
	var client = server.CreateClient();
	foreach (var rcpt in recipients) {
		client.Send(from, rcpt, $"Test message from {from} to {rcpt} via {server}", $"Test email from {from} to {rcpt} via {server}");
		Console.WriteLine($"Sent to {rcpt} via {server}");
	}
}
Console.WriteLine("Done! Press any key to exit.");
Console.ReadKey(true);
