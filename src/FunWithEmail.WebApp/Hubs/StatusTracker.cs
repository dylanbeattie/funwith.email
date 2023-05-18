using System.Collections.Concurrent;
using System.Text.Json;
using FunWithEmail.WebApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace FunWithEmail.WebApp.Hubs;

public class StatusTracker {
	private readonly IHubContext<MailHub> hubContext;
	private readonly ConcurrentDictionary<Guid, EmailStatus> statuses;

	public StatusTracker(IHubContext<MailHub> hubContext) {
		this.hubContext = hubContext;
		this.statuses = new();
	}

	public IDictionary<Guid, EmailStatus> Statuses => statuses;

	public async Task UpdateStatus(Guid id, EmailStatus status) {
		statuses.AddOrUpdate(id, status, (_, _) => status);
		var data = new { id, status = status.ToString() };
		var json = JsonSerializer.Serialize(data);
		await hubContext.Clients.All.SendAsync("UpdateStatus", "hub", json);
	}
}
