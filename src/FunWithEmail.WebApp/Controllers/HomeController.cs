using System.Diagnostics;
using FunWithEmail.WebApp.Models;
using FunWithEmail.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunWithEmail.WebApp.Controllers;

public class HomeController : Controller {
	private readonly ILogger<HomeController> logger;
	private readonly Dictionary<Guid, EmailState> states;
	private readonly Dictionary<string, SmtpConfiguration> smtpConfigurations;
	private readonly BackgroundTaskQueue tasks;

	public HomeController(ILogger<HomeController> logger,
		Dictionary<Guid, EmailState> states,
		Dictionary<string, SmtpConfiguration> smtpConfigurations,
		BackgroundTaskQueue tasks) {
		this.logger = logger;
		this.states = states;
		this.smtpConfigurations = smtpConfigurations;
		this.tasks = tasks;
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
			await tasks.EnqueueTaskAsync(async () => await SendMail(counter));
		}

		return Content("yeah!");
	}

	private async Task SendMail(int counter) {
		Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ":" + counter);
		await Task.Yield();
	}

	public IActionResult Privacy() {
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() {
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
