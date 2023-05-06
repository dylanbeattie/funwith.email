using FunWithEmail.WebApp.Models;
using FunWithEmail.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var smtpServers = new Dictionary<string, SmtpConfiguration>();
builder.Configuration.Bind("SmtpServers", smtpServers);
var states = new Dictionary<Guid, EmailState>();
builder.Services.AddSingleton(new BackgroundTaskQueue());

builder.Services.AddHostedService<TaskRunner>();
builder.Services.AddHostedService<TaskRunner>();
builder.Services.AddHostedService<TaskRunner>();
builder.Services.AddHostedService<TaskRunner>();

builder.Services.AddSingleton(states);
builder.Services.AddSingleton(smtpServers);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
