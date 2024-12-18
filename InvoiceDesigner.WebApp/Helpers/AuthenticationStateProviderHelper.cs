using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;


namespace InvoiceDesigner.WebApp.Helpers
{
	public class AuthenticationStateProviderHelper : AuthenticationStateProvider
	{
		private readonly ILocalStorageService _service;

		public AuthenticationStateProviderHelper(ILocalStorageService service)
		{
			_service = service;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var jwtToken = string.Empty;
			try
			{
				jwtToken = await _service.GetItemAsync<string>("jwtToken");

			}
			catch (Exception) { }
			if (string.IsNullOrEmpty(jwtToken))
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));


			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(jwtToken), "JwtAuth")));
		}

		private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			if (string.IsNullOrWhiteSpace(jwt))
				throw new ArgumentException("JWT cannot be null or empty.", nameof(jwt));

			var segments = jwt.Split('.');
			if (segments.Length != 3)
				throw new ArgumentException("Invalid JWT format.", nameof(jwt));

			var payload = segments[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);

			Dictionary<string, object>? keyValuePairs;
			try
			{
				keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
			}
			catch (JsonException ex)
			{
				throw new InvalidOperationException("Failed to deserialize JWT payload.", ex);
			}

			if (keyValuePairs == null)
				throw new InvalidOperationException("JWT payload is empty.");

			return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
		}

		private static byte[] ParseBase64WithoutPadding(string base64)
		{
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}
			return Convert.FromBase64String(base64);
		}
	}
}