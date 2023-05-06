using System.Threading.Channels;

namespace FunWithEmail.WebApp.Services;
public class BackgroundTaskQueue {
	private const int MAX_TASKS_IN_QUEUE = 500;
	private readonly Channel<Func<Task>> queue;

	public BackgroundTaskQueue() {
		var options = new BoundedChannelOptions(MAX_TASKS_IN_QUEUE) {
			FullMode = BoundedChannelFullMode.Wait
		};
		queue = Channel.CreateBounded<Func<Task>>(options);
	}

	public async ValueTask EnqueueTaskAsync(Func<Task> workItem)
		=> await queue.Writer.WriteAsync(workItem);

	public async ValueTask<Func<Task>> DequeueTaskAsync()
		=> await queue.Reader.ReadAsync();
}
