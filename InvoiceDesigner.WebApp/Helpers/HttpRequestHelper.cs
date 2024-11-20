using Blazored.LocalStorage;
using InvoiceDesigner.Domain.Shared.Responses;
using InvoiceDesigner.WebApp.Components.Pages;
using InvoiceDesigner.WebApp.Components.Pages.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Headers;

namespace InvoiceDesigner.WebApp.Helpers
{
	public class HttpRequestHelper
	{
		private readonly string _controller;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ISnackbar _snackbar;
		private readonly ILocalStorageService _localStorageService;
		private readonly NavigationManager? _navigationManager;
		private readonly IDialogService? _dialogService;

		public HttpRequestHelper(string controller,
									IHttpClientFactory HttpClientFactory,
									ISnackbar snackbar,
									ILocalStorageService localStorageService,
									NavigationManager? navigationManager = null,
									IDialogService? dialogService = null)
		{
			_controller = controller;
			_httpClientFactory = HttpClientFactory;
			_snackbar = snackbar;
			_localStorageService = localStorageService;
			_navigationManager = navigationManager;
			_dialogService = dialogService;
		}

		public async Task<byte[]?> DownloadPdf(string url)
		{
			try
			{
				var httpClient = await CreateHttpClient();
				var response = await httpClient.GetAsync($"api/{url}");
				switch (response.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						return await response.Content.ReadAsByteArrayAsync();
					case System.Net.HttpStatusCode.Forbidden:
						_snackbar.Add("Access Denied", Severity.Warning);
						break;
					case System.Net.HttpStatusCode.Unauthorized:
						_navigationManager?.NavigateTo("/Login", true);
						break;
					default:
						await ShowError(response);
						break;
				}
			}
			catch (Exception ex)
			{
				_snackbar.Add($"{ex.Message}", Severity.Error);
			}
			return null;
		}

		public async Task<bool> DeleteOrMarkAdDeletedAsync(int id)
		{
			try
			{
				if (_dialogService == null)
				{
					_snackbar.Add($"IDialogService is null", Severity.Error);
					return false;
				}

				var options = new DialogOptions { CloseOnEscapeKey = true };
				var dialog = await _dialogService.ShowAsync<DeleteEntity_dialog>(string.Empty, options);
				var result = await dialog.Result;

				if (result == null || result.Canceled)
				{
					return false;
				}

				if (!int.TryParse(result.Data?.ToString(), out int modeDelete))
					modeDelete = 0;

				var httpClient = await CreateHttpClient();
				var response = await httpClient.DeleteAsync($"api/{_controller}/{id}/{modeDelete}");

				switch (response.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						var res = await response.Content.ReadFromJsonAsync<ResponseBoolean>();

						if (res != null)
							return res.Result;

						_snackbar.Add("Error Read Response<ResponseBoolean>", Severity.Warning);
						return false;
					case System.Net.HttpStatusCode.Forbidden:
						_snackbar.Add("Access Denied", Severity.Warning);
						return false;
					case System.Net.HttpStatusCode.Unauthorized:
						_navigationManager?.NavigateTo("/Login", true);
						return false;
					default:
						await ShowError(response);
						return false;
				}
			}
			catch (Exception ex)
			{
				_snackbar.Add($"{ex.Message}", Severity.Error);
			}
			return false;
		}

		public async Task<bool> DeleteWithConfirmationAsync(int id)
		{
			try
			{
				if (_dialogService == null)
				{
					_snackbar.Add($"IDialogService is null", Severity.Error);
					return false;
				}


				var options = new DialogOptions { CloseOnEscapeKey = true };
				var dialog = await _dialogService.ShowAsync<ConfirmDelete>(string.Empty, options);
				var result = await dialog.Result;

				if (result == null || result.Canceled)
				{
					return false;
				}
				var httpClient = await CreateHttpClient();
				var response = await httpClient.DeleteAsync($"api/{_controller}/{id}");

				switch (response.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						var res = await response.Content.ReadFromJsonAsync<ResponseBoolean>();

						if (res != null)
							return res.Result;

						_snackbar.Add("Error Read Response<ResponseBoolean>", Severity.Warning);
						return false;
					case System.Net.HttpStatusCode.Forbidden:
						_snackbar.Add("Access Denied", Severity.Warning);
						return false;
					case System.Net.HttpStatusCode.Unauthorized:
						_navigationManager?.NavigateTo("/Login", true);
						return false;
					default:
						await ShowError(response);
						return false;
				}
			}
			catch (Exception ex)
			{
				_snackbar.Add($"{ex.Message}", Severity.Error);
			}
			return false;
		}

		public async Task<T?> GetAsync<T>(int? id) where T : new()
		{
			if (id.HasValue && id > 0)
			{
				var httpClient = await CreateHttpClient();
				var response = await httpClient.GetAsync($"api/{_controller}/{id}");

				switch (response.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						return await response.Content.ReadFromJsonAsync<T>();
					case System.Net.HttpStatusCode.Forbidden:
						_snackbar.Add("Access Denied", Severity.Warning);
						break;
					case System.Net.HttpStatusCode.Unauthorized:
						_navigationManager?.NavigateTo("/Login", true);
						break;
					default:
						await ShowError(response);
						break;
				}
			}
			return new T();
		}

		public async Task<int?> SendRequest<T>(T model, bool isUpdate, string? navUrl = null)
		{
			try
			{
				var httpClient = await CreateHttpClient();
				HttpResponseMessage response = isUpdate
												? await httpClient.PutAsJsonAsync($"api/{_controller}", model)
												: await httpClient.PostAsJsonAsync($"api/{_controller}", model);

				switch (response.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						var result = await response.Content.ReadFromJsonAsync<ResponseRedirect>();
						if (!string.IsNullOrEmpty(result?.RedirectUrl))
						{
							_navigationManager?.NavigateTo($"{result.RedirectUrl}", true);
						}
						else
						{
							_snackbar.Add($"Saved", Severity.Success);
							return result?.entityId;
						}
						break;
					case System.Net.HttpStatusCode.Forbidden:
						_snackbar.Add("Access Denied", Severity.Warning);
						break;
					case System.Net.HttpStatusCode.Unauthorized:
						_navigationManager?.NavigateTo("/Login", true);
						break;
					default:
						await ShowError(response);
						break;
				}
			}
			catch (Exception ex)
			{
				_snackbar.Add($"{ex.Message}", Severity.Error);
			}
			return null;
		}

		public async Task<T> GetDataFromAPI<T>(string url)
		{
			var httpClient = await CreateHttpClient();
			HttpResponseMessage response = await httpClient.GetAsync(url);
			try
			{
				switch (response.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						var result = await response.Content.ReadFromJsonAsync<T>();
						if (result != null)
							return result;
						break;
					case System.Net.HttpStatusCode.Forbidden:
						_snackbar.Add("Access Denied", Severity.Warning);
						break;
					case System.Net.HttpStatusCode.Unauthorized:
						_navigationManager?.NavigateTo("/Login", true);
						break;
				}
			}
			catch (Exception ex)
			{
				_snackbar.Add($"{ex.Message}", Severity.Error);
				throw;
			}
			throw new InvalidOperationException($"Error loading data: {await ShowError(response)}");
		}

		private async Task<string> ShowError(HttpResponseMessage response)
		{
			var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
			var message = errorContent?.Message ?? await response.Content.ReadAsStringAsync();
			_snackbar.Add($"{message}", Severity.Error);
			return message;
		}

		private class ErrorResponse
		{
			public string? Message { get; set; }
			public object? errors { get; set; }
		}

		public async Task<HttpClient> CreateHttpClient()
		{
			var httpClient = _httpClientFactory.CreateClient("ApiClient");
			var jwtToken = string.Empty;
			try
			{
				jwtToken = await _localStorageService.GetItemAsync<string>("jwtToken");

			}
			catch (Exception) { }
			if (!string.IsNullOrEmpty(jwtToken))
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

			return httpClient;
		}
	}
}
