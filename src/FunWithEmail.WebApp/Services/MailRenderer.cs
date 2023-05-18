using FunWithEmail.WebApp.Models;
using MimeKit;
using MimeKit.Utils;

namespace FunWithEmail.WebApp.Services;

public class MailRenderer {
	private readonly string htmlTemplate;

	public MailRenderer(string htmlTemplate) {
		this.htmlTemplate = htmlTemplate;
	}

	public MimeEntity MakeMailBody(MailItem item, SmtpSettings smtp) {
		var websiteBaseUrl = smtp.TestMode ? "http://localhost:5102" : "https://funwith.email";
		var confirmationUrl = $"{websiteBaseUrl}/go/confirm/{item.Id:N}";
		var bb = new BodyBuilder();
		var logo = bb.LinkedResources.Add("funwithemail.png");
		logo.ContentId = MimeUtils.GenerateMessageId("funwith.email");

		var tokens = new Dictionary<string, string> {
			{ "__LOGO_CONTENT_ID__", logo.ContentId },
			{ "__RECIPIENT__", item.Recipient.Address },
			{ "__SMTP_RELAY__", smtp.Host },
			{ "__CONFIRMATION_URL__", confirmationUrl }
		};

		var text = textTemplate;
		var html = htmlTemplate;
		foreach (var token in tokens) {
			html = html.Replace(token.Key, token.Value);
			text = text.Replace(token.Key, token.Value);
		}
		bb.TextBody = text;
		bb.HtmlBody = html;
		return bb.ToMessageBody();
	}

	private readonly string textTemplate = @"
		Hello __RECIPIENT__,

		It worked! You got an email! Isn't that cool?

		Now, click here to let us know you got it:

		__CONFIRMATION_URL__

		Thanks,

		Dylan

		PS: if you're interested, this message was sent via SMTP relay at __SMTP_RELAY__";
}
