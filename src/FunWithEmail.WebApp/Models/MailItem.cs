using MimeKit;

namespace FunWithEmail.WebApp.Models;

public class MailItem {
	public Guid Id { get; set; } = Guid.NewGuid();
	public MailboxAddress Recipient { get; set; }
	public MailItem(MailboxAddress recipient) {
		Recipient = recipient;
	}

	public override string ToString() => $"{Id}: {Recipient}";
}
