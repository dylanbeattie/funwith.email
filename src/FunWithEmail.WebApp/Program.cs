using FunWithEmail.WebApp.Hubs;
using FunWithEmail.WebApp.Services;
using Mjml.Net;

var builder = WebApplication.CreateBuilder(args);
var mvc = builder.Services.AddControllersWithViews();
if (builder.Environment.IsDevelopment()) mvc.AddRazorRuntimeCompilation();
builder.Logging.AddConsole();
builder.Services.AddSingleton<MailQueue>();

var mjmlRenderer = new MjmlRenderer();
var mjml = File.ReadAllText("Templates/FunWithEmail.mjml");
var (html, _) = mjmlRenderer.Render(mjml);
builder.Services.AddSingleton<MailRenderer>(services => {
	var env = services.GetService<IWebHostEnvironment>();
	return new(html, env?.IsDevelopment() ?? false);
});

builder.Services.AddSingleton<StatusTracker>();
builder.AddSmtpServices();
builder.Services.AddSignalR();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
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
