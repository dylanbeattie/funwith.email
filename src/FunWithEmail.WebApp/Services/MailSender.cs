using FunWithEmail.Common;
using FunWithEmail.WebApp.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace FunWithEmail.WebApp.Services; 

public class MailSender : BackgroundService {
	private readonly MailboxAddress from = new("Fun with Email", "hello@funwith.email");
	private readonly ILogger<MailSender> logger;
	private readonly MailRenderer renderer;
	private readonly SmtpSettings smtp;
	private readonly string smtpName;
	private readonly StatusTracker statusTracker;

	public MailSender(MailQueue queue, string smtpName, SmtpSettings smtp, MailRenderer renderer,
		StatusTracker statusTracker,
		ILogger<MailSender> logger) {
		MailQueue = queue;
		this.smtpName = smtpName;
		this.smtp = smtp;
		this.renderer = renderer;
		this.statusTracker = statusTracker;
		this.logger = logger;
		logger.LogDebug($"MailSender() with {smtp}");
	}

	public MailQueue MailQueue { get; }

	protected override async Task ExecuteAsync(CancellationToken token) {
		await Process(token);
	}

	private async Task Process(CancellationToken token) {
		while (!token.IsCancellationRequested) {
			var mailItem = await MailQueue.GetNextQueuedEmail();
			try {
				logger.LogDebug($"[Thread {Thread.CurrentThread.ManagedThreadId} {smtp} {mailItem}]");
				await SendMail(mailItem);
				await statusTracker.MarkAsSent(mailItem.Id, smtpName);
			} catch (Exception ex) {
				await statusTracker.MarkAsFailed(mailItem.Id, ex);
				logger.LogError(ex, "Error occurred executing {mailbox}.", mailItem);
			}
		}
	}

	private async Task SendMail(MailItem mailItem) {
		var smtpClient = new SmtpClient();
		await smtpClient.ConnectAsync(smtp.Host, smtp.Port);
		if (smtp.Username != null) await smtpClient.AuthenticateAsync(smtp.Username, smtp.Password);
		var mail = CreateMessage(mailItem);
		await smtpClient.SendAsync(mail);
	}

	public override async Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation($"Queued Hosted Service is starting for {smtp}.");
		await base.StartAsync(cancellationToken);
	}

	public override async Task StopAsync(CancellationToken stoppingToken) {
		logger.LogInformation($"The {smtp.Host} worker service is stopping...");
		await base.StopAsync(stoppingToken);
		logger.LogInformation($"The {smtp.Host} worker service is stopped.");
	}

	private MimeMessage CreateMessage(MailItem mailItem) {
		var mail = new MimeMessage { Subject = "Fun with Email!" };
		mail.From.Add(from);
		mail.To.Add(mailItem.Recipient);
		mail.Body = renderer.MakeMailBody(mailItem, smtp);
		return mail;
	}
}
