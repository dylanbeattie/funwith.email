using System.Diagnostics;
using FunWithEmail.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FunWithEmail.WebApp.Controllers; 

public class HomeController : Controller {
	private readonly ILogger<HomeController> logger;

	public HomeController(ILogger<HomeController> logger) {
		this.logger = logger;
	}

	public IActionResult Index() {
		return View();
	}

	public IActionResult Privacy() {
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() {
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
