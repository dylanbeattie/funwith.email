using System.Diagnostics;

namespace FunWithEmail.WebApp.Services;

public class MailSender : BackgroundService {
	private readonly SmtpSettings smtp;
	private readonly ILogger<MailSender> logger;

	public MailSender(MailQueue queue, string name, SmtpSettings smtp, ILogger<MailSender> logger) {
		MailQueue = queue;
		this.smtp = smtp;
		this.logger = logger;
		logger.LogDebug($"MailSender() with {smtp}");
	}

	public MailQueue MailQueue { get; }

	protected override async Task ExecuteAsync(CancellationToken token) {
		await Process(token);
	}

	private async Task Process(CancellationToken token) {
		while (!token.IsCancellationRequested) {
			await Task.Delay(TimeSpan.FromSeconds(1), token);
			var mailbox = MailQueue.GetNextQueuedEmail();
			if (mailbox == null) continue;
			try {
				logger.LogDebug($"[Thread {Thread.CurrentThread.ManagedThreadId} {smtp} {mailbox}");
			} catch (Exception ex) {
				logger.LogError(ex, "Error occurred executing {mailbox}.", mailbox);
			}
		}
	}

	public override async Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation($"Queued Hosted Service is starting for {smtp}.");
		await base.StartAsync(cancellationToken);
	}

	public override async Task StopAsync(CancellationToken stoppingToken) {
		logger.LogInformation("Queued Hosted Service is stopping.");
		await base.StopAsync(stoppingToken);
	}
}
