using FunWithEmail.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunWithEmail.WebApp.Controllers {
	public class GoController : Controller {
		private readonly ILogger<GoController> logger;
		private readonly MailQueue queue;
		private readonly StatusTracker tracker;

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
				var id = await tracker.Create(email);
				return RedirectToAction(nameof(Wait), new { id });
			} catch (Exception ex) {
				ViewData["Error"] = ex.Message;
				return View();
			}
		}

		[HttpGet]
		public IActionResult Wait(Guid id) {
			if (tracker.TryGetItem(id, out var item)) return View(item);
			return NotFound();
		}

		public async Task<IActionResult> Queue(Guid id) {
			if (!tracker.TryGetItem(id, out var item)) return RedirectToAction(nameof(Index));
			await queue.AddEmailToQueue(item);
			await tracker.MarkAsQueued(id);
			return View(item);
		}

		public async Task<IActionResult> Confirm(Guid id, bool junk) {
			if (junk) await tracker.MarkAsDeliveredToJunk(id);
			else await tracker.MarkAsDeliveredToInbox(id);
			return View();
		}
	}
}
