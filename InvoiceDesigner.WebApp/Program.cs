using Blazored.LocalStorage;
using InvoiceDesigner.Localization;
using InvoiceDesigner.Localization.Resources;
using InvoiceDesigner.WebApp.Components;
using InvoiceDesigner.WebApp.Helpers;
using InvoiceDesigner.WebClient;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddLocalization();
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddMudServices(config =>
{
	config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
	config.SnackbarConfiguration.PreventDuplicates = false;
	config.SnackbarConfiguration.NewestOnTop = false;
	config.SnackbarConfiguration.ShowCloseIcon = true;
	config.SnackbarConfiguration.VisibleStateDuration = 1000;
	config.SnackbarConfiguration.HideTransitionDuration = 200;
	config.SnackbarConfiguration.ShowTransitionDuration = 200;
	config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

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

#region HttpRequestHelper
builder.Services.AddScoped<HttpRequestHelper>(sp =>
{
	var queryHelper = new QueryHttpRequestHelper
	{
		HttpClientFactory = sp.GetRequiredService<IHttpClientFactory>(),
		Snackbar = sp.GetRequiredService<ISnackbar>(),
		LocalStorageService = sp.GetRequiredService<ILocalStorageService>(),
		Nav = sp.GetRequiredService<NavigationManager>(),
		DialogService = sp.GetRequiredService<IDialogService>(),
		localizer = sp.GetRequiredService<IStringLocalizer<Resource>>()
	};

	return new HttpRequestHelper(queryHelper);
});
#endregion

var app = builder.Build();

app.MapDefaultEndpoints();



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

#region Localization
var supportedCultures = Locale.SupportedCultures.Select(c => c.Name).ToArray();
var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture(supportedCultures[0])
	.AddSupportedCultures(supportedCultures)
	.AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
#endregion

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
