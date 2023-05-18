using System.Collections.Concurrent;
using FunWithEmail.WebApp.Hubs;
using FunWithEmail.WebApp.Models;
using FunWithEmail.WebApp.Services;
using Microsoft.AspNetCore.SignalR;
using Mjml.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Logging.AddConsole();

var smtpServers = new Dictionary<string, SmtpSettings>();
builder.Configuration.Bind("Smtp", smtpServers);
builder.Services.AddSingleton<MailQueue>();

var mjmlRenderer = new MjmlRenderer();
var mjml = File.ReadAllText("Templates/FunWithEmail.mjml");
var (html, _) = mjmlRenderer.Render(mjml);
var mailRenderer = new MailRenderer(html);
builder.Services.AddSingleton(mailRenderer);
builder.Services.AddSingleton<StatusTracker>();
foreach (var server in smtpServers) {
	Console.WriteLine("Creating SMTP relay worker for " + server.Key);
	builder.Services.AddSingleton<IHostedService>(provider => {
		var logger = provider.GetService<ILogger<MailSender>>();
		var queue = provider.GetService<MailQueue>();
		var renderer = provider.GetService<MailRenderer>();
		var tracker = provider.GetService<StatusTracker>();
		var sender = new MailSender(queue!, server.Key, server.Value, renderer!, tracker!, logger!);
		return sender;
	});
}

var states = new ConcurrentDictionary<Guid, EmailState>();
builder.Services.AddSingleton(states);
builder.Services.AddSignalR();

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
	pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapHub<MailHub>("/hub");
app.Run();
