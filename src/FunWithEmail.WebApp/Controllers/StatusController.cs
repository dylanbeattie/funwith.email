using System.Collections.Concurrent;
using FunWithEmail.WebApp.Hubs;
using FunWithEmail.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FunWithEmail.WebApp.Controllers;

public class StatusController : Controller {
	private readonly StatusTracker tracker;
	private readonly ILogger<StatusController> logger;

	public StatusController(StatusTracker tracker, ILogger<StatusController> logger) {
		this.tracker = tracker;
		this.logger = logger;
	}

	public IActionResult Index() => View(tracker.Statuses);
}
