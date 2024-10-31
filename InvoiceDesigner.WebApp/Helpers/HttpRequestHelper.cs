using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.WebApp.Components.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InvoiceDesigner.WebApp.Helpers
{
	public class HttpRequestHelper
	{
		private readonly string _controller;
		private readonly HttpClient _httpClient;
		private readonly ISnackbar _snackbar;
		private readonly NavigationManager? _navigationManager;
		private readonly IDialogService? _dialogService;

		public HttpRequestHelper(string controller, HttpClient httpClient, ISnackbar snackbar, NavigationManager? navigationManager = null, IDialogService? dialogService = null)
		{
			_controller = controller;
			_httpClient = httpClient;
			_snackbar = snackbar;
			_navigationManager = navigationManager;
			_dialogService = dialogService;
		}

		public async Task<bool> DeleteWithConfirmationAsync(int id)
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

			var response = await _httpClient.DeleteAsync($"api/{_controller}/{id}");

			if (!response.IsSuccessStatusCode)
			{
				await ShowError(response);
				return false;
			}
			return true;
		}

		public async Task<T?> GetAsync<T>(int? id) where T : new()
		{
			if (id.HasValue && id > 0)
			{
				var response = await _httpClient.GetAsync($"api/{_controller}/{id}");
				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadFromJsonAsync<T>();
				}
				else
				{
					_navigationManager?.NavigateTo($"/{_controller}");
					return default;
				}
			}
			else
			{
				return new T();
			}
		}

		public async Task SendRequest<T>(T model, bool isUpdate, string? navUrl = null)
		{
			try
			{
				HttpResponseMessage response = isUpdate
												? await _httpClient.PutAsJsonAsync($"api/{_controller}", model)
												: await _httpClient.PostAsJsonAsync($"api/{_controller}", model);

				if (!response.IsSuccessStatusCode)
				{
					throw new InvalidOperationException($"Error loading data: {await ShowError(response)}");
				}
				else
				{
					var result = await response.Content.ReadFromJsonAsync<ResponseRedirect>();
					if (result != null && !string.IsNullOrEmpty(result.RedirectUrl))
						_navigationManager?.NavigateTo($"{result.RedirectUrl}", true);
					else
						_snackbar.Add($"Saved", Severity.Success); 
				}
			}
			catch (Exception ex)
			{
				_snackbar.Add($"{ex.Message}", Severity.Error);
			}
		}

		public async Task<T> GetDataFromAPI<T>(string url)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<T>();
					if (result == null)
					{
						_snackbar.Add("Error: ReadFromJsonAsync is null", Severity.Error);
						throw new InvalidOperationException("Error: ReadFromJsonAsync is null");
					}
					return result;
				}
				else
				{
					throw new InvalidOperationException($"Error loading data: {await ShowError(response)}");
				}
			}
			catch (Exception ex)
			{
				_snackbar.Add($"{ex.Message}", Severity.Error);
				throw;
			}
		}

		private async Task<string> ShowError(HttpResponseMessage response)
		{
			var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
			var message = errorContent?.Message ?? await response.Content.ReadAsStringAsync();
			_snackbar.Add($"{message}", Severity.Error);
			return message;
		}

	}

	public class ErrorResponse
	{
		public string? Message { get; set; }
		public object? errors { get; set; }
	}

}
