using System.Collections.Concurrent;
using System.Reflection.Metadata;
using System.Text.Json;
using FunWithEmail.Common;
using FunWithEmail.WebApp.Hubs;
using FunWithEmail.WebApp.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.SignalR;
using MimeKit;

namespace FunWithEmail.WebApp.Services;

public class StatusTracker {
	private readonly IConfiguration config;

	private readonly IHubContext<MailHub> hubContext;
	private readonly ConcurrentDictionary<Guid, MailItem> items;

	public StatusTracker(IHubContext<MailHub> hubContext, IConfiguration config) {
		this.hubContext = hubContext;
		this.config = config;
		items = new();
	}

	public IEnumerable<MailItem> AllItems => items.Values;

	public IDictionary<Guid, MailStatus> Statuses
		=> items.ToDictionary(item => item.Key, item => item.Value.Status);

	private void MakeFakeData() {
		var random = new Random();
		var smtpServers = new Dictionary<string, SmtpSettings>();
		var statuses = Enum.GetValues<MailStatus>();
		config.Bind("Smtp", smtpServers);
		var relays = smtpServers.Keys.Concat(new[] { String.Empty }).ToArray();
		for (var i = 0; i < 250; i++) {
			var guid = Guid.NewGuid();
			var item = new MailItem(guid, $"test{i:0000}@example.com") {
				Status = statuses[random.Next(statuses.Length)],
				SmtpRelay = relays[random.Next(relays.Length)]
			};
			items[guid] = item;
		}
	}

	public bool TryGetItem(Guid id, out MailItem item) {
		return items.TryGetValue(id, out item);
	}

	public async ValueTask<Guid> Create(string emailAddress) {
		var id = Guid.NewGuid();
		items[id] = new(id, emailAddress);
		await Update(id, item => item.Status = MailStatus.Accepted);
		return id;
	}

	private async Task Update(Guid id, Action<MailItem> action, Exception? ex = null) {
		if (!items.TryGetValue(id, out var item)) return;
		action(item);
		var data = new {
			id,
			status = item.Status.ToString().ToLowerInvariant(),
			message = ex?.Message ?? String.Empty
		};
		var json = JsonSerializer.Serialize(data);
		await hubContext.Clients.All.SendAsync("UpdateStatus", "hub", json);
	}

	public async Task MarkAsSent(Guid id, string smtpRelay) {
		await Update(id, item => {
			item.SmtpRelay = smtpRelay;
			item.Status = MailStatus.Sent;
		});
	}

	public async Task MarkAsDeliveredToInbox(Guid id) {
		await Update(id, item => item.Status = MailStatus.DeliveredToInbox);
	}

	public async Task MarkAsDeliveredToJunk(Guid id) {
		await Update(id, item => item.Status = MailStatus.DeliveredToJunk);
	}

	public async Task MarkAsFailed(Guid id, Exception ex) {
		await Update(id, item => item.Status = MailStatus.Error , ex);
	}

	public async Task MarkAsQueued(Guid id) {
		await Update(id, item => item.Status = MailStatus.Queued);
	}

	public void Fake() => MakeFakeData();

	public void Reset() => items.Clear();

	public async Task MarkAsInvalid(Guid id) {
		await Update(id, item => item.Status = MailStatus.Invalid);
	}

	private readonly MailboxAddress from = new("Fun with Email", "hello@funwith.email");

	public async Task<Dictionary<string,string>> TestRelaysAsync() {
		var result = new Dictionary<string, string>();
		var smtpServers = new Dictionary<string, SmtpSettings>();
		config.Bind("Smtp", smtpServers);
		foreach (var smtp in smtpServers.Values) {
			try {
				var smtpClient = new SmtpClient();
				await smtpClient.ConnectAsync(smtp.Host, smtp.Port);
				if (smtp.Username != null) await smtpClient.AuthenticateAsync(smtp.Username, smtp.Password);
				var mail = new MimeMessage { Subject = $"TEST via {smtp.Host} : Fun with Email!" };
				mail.From.Add(from);
				mail.To.Add(new MailboxAddress("dylan@dylanbeattie.net", "dylan@dylanbeattie.net"));
				var bb = new BodyBuilder {
					TextBody = $"Test mail via {smtp.Host}"
				};
				mail.Body = bb.ToMessageBody();
				await smtpClient.SendAsync(mail);
				result.Add(smtp.Host, "OK");
			} catch (Exception ex) {
				result.Add(smtp.Host, ex.Message);
			}
		}
		return result;
	}
}
