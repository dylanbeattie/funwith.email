using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace FunWithEmail.WebApp.Models; 

public class MailItem {
	public MailItem(Guid id, string emailAddress) {
		Id = id;
		Recipient = new(null, emailAddress);
	}

	[HiddenInput]
	public Guid Id { get; set; }

	public MailboxAddress Recipient { get; set; }
	public string? SmtpRelay { get; set; }
	public MailStatus Status { get; set; }
}
