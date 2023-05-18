using FunWithEmail.WebApp.Models;
using FunWithEmail.WebApp.Services;
using Microsoft.Extensions.Logging.Abstractions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Logging.AddConsole();

var smtpServers = new Dictionary<string, SmtpSettings>();
builder.Configuration.Bind("Smtp", smtpServers);
var states = new Dictionary<Guid, EmailState>();
builder.Services.AddSingleton<MailQueue>();
foreach (var server in smtpServers) {
	Console.WriteLine("Creating " + server.Key);
	builder.Services.AddSingleton<IHostedService>(provider => {
		var logger = provider.GetService<ILogger<MailSender>>();
		var queue = provider.GetService<MailQueue>();
		var sender = new MailSender(queue!, server.Key, server.Value, logger!);
		return sender;
	});
}
builder.Services.AddSingleton(states);
var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
