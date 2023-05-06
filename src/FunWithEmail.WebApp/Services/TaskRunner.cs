namespace FunWithEmail.WebApp.Services;

public class TaskRunner : BackgroundService {
	private readonly ILogger<TaskRunner> logger;

	public TaskRunner(BackgroundTaskQueue taskQueue, ILogger<TaskRunner> logger) {
		TaskQueue = taskQueue;
		this.logger = logger;
	}

	public BackgroundTaskQueue TaskQueue { get; }

	protected override async Task ExecuteAsync(CancellationToken token) {
		await BackgroundProcessing(token);
	}

	private async Task BackgroundProcessing(CancellationToken token) {
		var semaphore = new SemaphoreSlim(10);
		while (!token.IsCancellationRequested) {
			await semaphore.WaitAsync(token);
			var task = await TaskQueue.DequeueTaskAsync();
			task().ContinueWith(_ => semaphore.Release());
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken) {
		logger.LogInformation("Queued Hosted Service is stopping.");
		await base.StopAsync(stoppingToken);
	}
}
