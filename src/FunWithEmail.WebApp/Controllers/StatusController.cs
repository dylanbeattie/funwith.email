using FunWithEmail.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunWithEmail.WebApp.Controllers {
	public class StatusController : Controller {
		private readonly ILogger<StatusController> logger;
		private readonly StatusTracker tracker;

		public StatusController(StatusTracker tracker, ILogger<StatusController> logger) {
			this.tracker = tracker;
			this.logger = logger;
		}

		public IActionResult Index() {
			return View(tracker.Statuses);
		}

		public IActionResult Report() {
			var groups = tracker
				.AllItems
				.Where(item => !String.IsNullOrWhiteSpace(item.SmtpRelay))
				.GroupBy(item => item.SmtpRelay);
			return View(groups);
		}
	}
}
