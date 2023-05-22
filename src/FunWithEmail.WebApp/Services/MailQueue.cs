using System.Threading.Channels;
using FunWithEmail.WebApp.Models;

namespace FunWithEmail.WebApp.Services {
	public class MailQueue {
		private readonly ILogger<MailQueue> logger;
		private readonly Channel<MailItem> queue;

		public MailQueue(ILogger<MailQueue> logger) {
			this.logger = logger;
			var options = new BoundedChannelOptions(250) { FullMode = BoundedChannelFullMode.Wait };
			queue = Channel.CreateBounded<MailItem>(options);
		}

		public async Task AddEmailToQueue(MailItem mailbox) {
			logger.LogDebug("Queueing " + mailbox);
			await queue.Writer.WriteAsync(mailbox);
		}

		public async Task<MailItem> GetNextQueuedEmail() {
			var item = await queue.Reader.ReadAsync();
			logger.LogDebug("Dequeued " + item);
			return item;
		}
	}
}
