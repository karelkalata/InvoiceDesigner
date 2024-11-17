namespace InvoiceDesigner.Application.Helpers.Currency
{

	public static class CurrencyUnicodeHEX
	{
		private static readonly Dictionary<string, string> _currenciesUnicodeHEX = new Dictionary<string, string>
	{
		{ "USD", "\u0024" },
		{ "EUR", "\u20AC" },
		{ "GBP", "\u00A3" },
		{ "JPY", "\u00A5" },
		{ "AUD", "\u0024" },
		{ "CAD", "\u0024" },
		{ "CHF", "CHF" },
		{ "CNY", "¥" },
		{ "SEK", "kr" },
		{ "NZD", "\u0024" },
		{ "MXN", "\u0024" },
		{ "SGD", "\u0024" },
		{ "HKD", "\u0024" },
		{ "NOK", "kr" },
		{ "KRW", "\u20A9" },
		{ "TRY", "\u20BA" },
		{ "RUB", "\u20BD" },
		{ "INR", "\u20B9" },
		{ "BRL", "R$" },
		{ "ZAR", "R" },
		{ "CZK", "Kč" },
		{ "PLN", "zł" },
		{ "DKK", "kr" },
		{ "HUF", "Ft" },
		{ "ILS", "\u20AA" },
		{ "THB", "\u0E3F" },
		{ "PHP", "\u20B1" },
		{ "MYR", "RM" },
		{ "IDR", "Rp" },
		{ "VND", "\u20AB" },
		{ "AED", "د.إ" },
		{ "SAR", "﷼" },
		{ "KWD", "K.D." },
		{ "OMR", "﷼" },
		{ "QAR", "﷼" },
		{ "EGP", "£" },
		{ "NGN", "₦" },
		{ "PEN", "S/." },
		{ "ARS", "\u0024" },
		{ "CLP", "\u0024" },
		{ "COP", "\u0024" },
		{ "UYU", "$U" }
	};

		public static string GetCurrencyUnicodeHEX(string currencyCode)
		{
			return _currenciesUnicodeHEX.TryGetValue(currencyCode, out var symbol)
				? symbol
				: string.Empty;
		}

	}

}
