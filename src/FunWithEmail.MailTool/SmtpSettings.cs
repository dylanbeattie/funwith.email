using System.Net;
using System.Net.Mail;

public class SmtpSettings {
	public string Host { get; set; } = "";
	public int Port { get; set; } = 25;
	public string? Username { get; set; }
	public string? Password { get; set; }
	public bool EnableSsl { get; set; }
	public override string ToString() => $"{Host}:{Port}";

	public SmtpClient CreateClient() {
		var client = new SmtpClient(Host, Port);
		if (String.IsNullOrEmpty(Username)) return client;
		client.Credentials = new NetworkCredential(Username, Password);
		if (EnableSsl) client.EnableSsl = true;
		return client;
	}
}
