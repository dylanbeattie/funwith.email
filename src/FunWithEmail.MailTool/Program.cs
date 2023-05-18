using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Utils;
using Mjml.Net;


IConfiguration config = new ConfigurationBuilder()
	.AddJsonFile("appSettings.json")
	.AddEnvironmentVariables()
	.AddUserSecrets(typeof(Program).Assembly)
	.Build();

var from = new MailboxAddress("Company Logo", "hello@funwith.email");
// var from = new MailboxAddress("Company Logo", "do-not-reply@company-logo.com");
var smtpServers = config.GetSection("Smtp").Get<Dictionary<string, SmtpSettings>>();
var recipients = new[] { "dylan@dylanbeattie.net" }; //  "dylan.beattie@gmail.com", "dylan@funwith.email", "dylan@fm.funwith.email", };
var renderer = new MjmlRenderer();
var server = smtpServers["SocketLabs"];
// foreach (var server in smtpServers.Values) {
	var smtp = new SmtpClient();
	smtp.Connect(server.Host, server.Port);
	if (server.Username != null) smtp.Authenticate(server.Username, server.Password);
	foreach (var rcpt in recipients) {
		var mail = new MimeMessage();
		// mail.Subject = "Fun with Email!";
		mail.Subject = "Welcome to Company Logo";
		mail.From.Add(from);
		mail.To.Add(new MailboxAddress(rcpt, rcpt));
		var bb = new BodyBuilder();
		var text = $"Test email from {from} to {rcpt} via {server}";
		bb.TextBody = text;
//		var mjml = File.ReadAllText("Templates/Mail.mjml");
		var mjml = File.ReadAllText("Templates/CompanyLogo.mjml");
		var (html, errors) = renderer.Render(mjml);
		var logo = bb.LinkedResources.Add("company-logo-dark-mode.png");
		// var logo = bb.LinkedResources.Add("funwithemail.png");
		logo.ContentId = MimeUtils.GenerateMessageId();
		html = html.Replace("__LOGO_CONTENT_ID__", logo.ContentId);
		html = html.Replace("__RECIPIENT__", rcpt);
		html = html.Replace("__SMTP_RELAY__", server.ToString());
		bb.HtmlBody = html;
		mail.Body = bb.ToMessageBody();
		smtp.Send(mail);
		Console.WriteLine($"Sent to {rcpt} via {server}");
	}
	smtp.Disconnect(true);
// }
Console.WriteLine("Done! Press any key to exit.");
Console.ReadKey(true);


