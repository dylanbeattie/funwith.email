using FunWithEmail.Common;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

IConfiguration config = new ConfigurationBuilder()
	.AddJsonFile("appSettings.json")
	.AddEnvironmentVariables()
	.AddUserSecrets(typeof(Program).Assembly)
	.Build();

var from = new MailboxAddress("Fun with Email", "hello@funwith.email");
var smtpServers = config.GetSection("Smtp")
	.Get<Dictionary<string, SmtpSettings>>();

foreach (var entry in smtpServers) Console.WriteLine(entry.Key);
Console.WriteLine("Press a key to continue...");
Console.ReadKey();
foreach (var entry in smtpServers) {
	var domain = entry.Key;
	var server = entry.Value;
	Console.WriteLine($"Sending via {server.Host} - press 1 to skip");
	if (Console.ReadKey().KeyChar == '1') continue;
	var smtp = new SmtpClient();
	try {
		smtp.Connect(server.Host, server.Port);
		if (server.Username != null) smtp.Authenticate(server.Username, server.Password);
		var recipients = new[] {
			"dylan@dylanbeattie.net"
			//$"hello@{domain}",
			//$"\"with spaces\"@{domain}",
			//$"two..dots@{domain}",
			//$"escaped\\ space@{domain}"
		};
		foreach (var rcpt in recipients) {
			Console.WriteLine("=============================");
			Console.WriteLine($"Sending to {rcpt} via {server.Host}");
			try {
				var mail = new MimeMessage();
				mail.Subject = $"Fun with Email! {rcpt}";
				mail.From.Add(from);
				mail.To.Add(new MailboxAddress(rcpt, rcpt));
				var bb = new BodyBuilder();
				var text = $"Test email from {from} to {rcpt} via {server}";
				bb.TextBody = text;
				mail.Body = bb.ToMessageBody();
				smtp.Send(mail);
				Console.WriteLine($"Sent to {rcpt} via {server}");
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}

		smtp.Disconnect(true);
	} catch (Exception ex) {
		Console.Write($"Failed sending via {entry.Key} {server.Host} ({server.Username} / {server.Password}");
		Console.Write(ex.ToString());
	}
}

Console.WriteLine("Done! Press any key to exit.");
Console.ReadKey(true);
