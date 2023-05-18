public class SmtpSettings {
	public string Host { get; set; } = "";
	public int Port { get; set; } = 25;
	public string? Username { get; set; }
	public string? Password { get; set; }
	public bool EnableSsl { get; set; }
	public override string ToString() => $"{Host}:{Port}";
	public bool TestMode { get; set; } = false;
}
