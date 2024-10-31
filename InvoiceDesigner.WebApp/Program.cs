using InvoiceDesigner.WebApp.Components;
using InvoiceDesigner.WebClient;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped(sp =>
{
    var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
    if (apiSettings == null || string.IsNullOrEmpty(apiSettings.BaseUrl))
    {
        throw new InvalidOperationException("API base URL is not configured.");
    }
    return new HttpClient { BaseAddress = new Uri(apiSettings.BaseUrl) };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
