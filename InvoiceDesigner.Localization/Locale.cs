using System.Globalization;

namespace InvoiceDesigner.Localization
{
	public class Locale
	{
		public static CultureInfo[] SupportedCultures { get; set; } = new[]
		{
			new CultureInfo("en-US"),
			new CultureInfo("cs-CZ"),
			new CultureInfo("de-DE"),
			new CultureInfo("es-ES"),
			new CultureInfo("fr-FR"),
			new CultureInfo("it-IT"),
			new CultureInfo("pl-PL"),
			new CultureInfo("pt-PT"),
			new CultureInfo("ru-RU"),
		};
	}
}
