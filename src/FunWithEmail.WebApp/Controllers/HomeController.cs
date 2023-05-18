using System.Diagnostics;
using FunWithEmail.WebApp.Models;
using FunWithEmail.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace FunWithEmail.WebApp.Controllers;

public class HomeController : Controller {
	private readonly ILogger<HomeController> logger;
	private readonly Dictionary<Guid, EmailState> states;
	private readonly MailQueue queue;

	public HomeController(ILogger<HomeController> logger,
		Dictionary<Guid, EmailState> states,
		MailQueue queue) {
		this.logger = logger;
		this.states = states;
		this.queue= queue;
	}

	public IActionResult Index() {
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Index(string email) {
		var id = Guid.NewGuid();
		var state = new EmailState(email);
		states.Add(id, state);
		for (var counter = 0; counter < 100; counter++) {
			var mailbox = new MailboxAddress(String.Empty, $"test{counter}@dylanbeattie.net");
			logger.LogDebug(mailbox.Address);
			queue.AddEmailToQueue(mailbox);
		}
		return Content("yeah!");
	}

	public IActionResult Privacy() {
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() {
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
