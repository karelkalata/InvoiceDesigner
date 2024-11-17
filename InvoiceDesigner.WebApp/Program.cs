using Blazored.LocalStorage;
using InvoiceDesigner.WebApp.Components;
using InvoiceDesigner.WebApp.Helpers;
using InvoiceDesigner.WebClient;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationStateProviderHelper>();
builder.Services.AddAuthorizationCore();

builder.Services.AddHttpClient("ApiClient", client =>
{
	var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
	if (apiSettings == null || string.IsNullOrEmpty(apiSettings.BaseUrl))
	{
		throw new InvalidOperationException("API base URL is not configured.");
	}
	client.BaseAddress = new Uri(apiSettings.BaseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.Use(async (context, next) =>
{
	if (context.Request.Path == "/")
	{
		context.Response.Redirect("/Invoices");
		return;
	}
	await next();
});

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
