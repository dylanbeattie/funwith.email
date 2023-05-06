namespace FunWithEmail.WebApp.Models;

public class EmailState {
	public string EmailAddress { get; set; }
	public EmailStatus Status { get; set; }

	public EmailState(string emailAddress) {
		this.EmailAddress = emailAddress;
		this.Status = EmailStatus.Unknown;
	}
}
