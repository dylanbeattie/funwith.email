using System.Collections.Concurrent;
using System.Threading.Channels;
using MimeKit;
using static MailKit.Net.Imap.ImapMailboxFilter;

namespace FunWithEmail.WebApp.Services;

public class MailQueue {
	private readonly ILogger<MailQueue> logger;
	private readonly ConcurrentQueue<MailboxAddress> queue;

	public MailQueue(ILogger<MailQueue> logger) {
		this.logger = logger;
		queue = new();
	}

	public void AddEmailToQueue(MailboxAddress mailbox) {
		logger.LogDebug("Enqueuing " + mailbox);
		queue.Enqueue(mailbox);
	}

	public MailboxAddress? GetNextQueuedEmail() {
		if (!queue.TryDequeue(out var mailbox)) return null;
		logger.LogDebug("Dequeuing " + mailbox);
		return mailbox;
	}
}
