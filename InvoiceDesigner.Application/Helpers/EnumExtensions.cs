using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Application.Helpers
{
	public static class EnumExtensions
	{
		public static string GetEnumDisplayName(this Enum enumValue)
		{
			var displayAttribute = enumValue.GetType()
				.GetField(enumValue.ToString())
				?.GetCustomAttributes(false)
				.OfType<DisplayAttribute>()
				.FirstOrDefault();

			return displayAttribute?.Name ?? enumValue.ToString();
		}
	}
}
