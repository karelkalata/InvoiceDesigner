using System.Globalization;

namespace InvoiceDesigner.WebApp.Helpers
{
	public static class QueryStringExtensions
	{
		public static string ToQueryString(this object obj)
		{
			if (obj == null) return string.Empty;

			var properties = from p in obj.GetType().GetProperties()
							 let value = p.GetValue(obj)
							 where value != null
							 from serializedValue in SerializeProperty(p.Name, value)
							 select serializedValue;

			return string.Join("&", properties);
		}

		private static IEnumerable<string> SerializeProperty(string name, object value)
		{
			if (value is IEnumerable<int> list)
			{
				foreach (var item in list)
				{
					yield return $"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(item.ToString())}";
				}
			}
			else if (value is DateTime dateTime)
			{

				yield return $"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture))}";
			}
			else if (value != null && value.GetType() == typeof(DateTime?))
			{
				DateTime? dateTimeNullable = (DateTime?)value;
				if (dateTimeNullable.HasValue)
				{
					var formattedDate = dateTimeNullable.Value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
					yield return $"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(formattedDate)}";
				}
			}
			else
			{
				yield return $"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(value?.ToString() ?? string.Empty)}";
			}
		}
	}
}
