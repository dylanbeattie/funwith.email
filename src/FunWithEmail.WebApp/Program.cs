using FunWithEmail.WebApp.Models;
using FunWithEmail.WebApp.Services;
using Mjml.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Logging.AddConsole();

var smtpServers = new Dictionary<string, SmtpSettings>();
builder.Configuration.Bind("Smtp", smtpServers);
var states = new Dictionary<Guid, EmailState>();
builder.Services.AddSingleton<MailQueue>();

var mjmlRenderer = new MjmlRenderer();
var mjml = File.ReadAllText("Templates/FunWithEmail.mjml");
var (html, _) = mjmlRenderer.Render(mjml);
var mailRenderer = new MailRenderer(html);
builder.Services.AddSingleton(mailRenderer);
foreach (var server in smtpServers.Where(s => s.Value.TestMode)) {
	Console.WriteLine("Creating SMTP relay worker for " + server.Key);
	builder.Services.AddSingleton<IHostedService>(provider => {
		var logger = provider.GetService<ILogger<MailSender>>();
		var queue = provider.GetService<MailQueue>();
		var renderer = provider.GetService<MailRenderer>();
		var sender = new MailSender(queue!, server.Key, server.Value, renderer, logger!);
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
	pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.Run();
