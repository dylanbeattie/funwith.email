using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.Json;
using FunWithEmail.WebApp.Hubs;
using FunWithEmail.WebApp.Models;
using FunWithEmail.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MimeKit;

namespace FunWithEmail.WebApp.Controllers;

public class GoController : Controller {
	private readonly ILogger<GoController> logger;
	private readonly StatusTracker tracker;
	private readonly MailQueue queue;

	public GoController(ILogger<GoController> logger,
		StatusTracker tracker,
		MailQueue queue) {
		this.logger = logger;
		this.tracker = tracker;
		this.queue = queue;
	}

	public IActionResult Index() {
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Index(string email) {
		try {
			var mailbox = new MailboxAddress(email, email);
			logger.LogDebug(mailbox.Address);
			var item = new MailItem(mailbox);
			await tracker.UpdateStatus(item.Id, EmailStatus.Unknown);
			await queue.AddEmailToQueue(item);
			await tracker.UpdateStatus(item.Id, EmailStatus.Queued);
			return View("Sent");
		}
		catch (Exception ex) {
			ViewData["Error"] = ex.Message;
			return View();
		}
	}

	public async Task<IActionResult> Confirm(Guid id) {
		await tracker.UpdateStatus(id, EmailStatus.Verified);
		return View();
	}
}
