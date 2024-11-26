namespace InvoiceDesigner.Domain.Shared.Extensions
{
	public static class QueryStringExtensions
	{
		public static string ToQueryString(this object obj)
		{
			if (obj == null) return string.Empty;

			var properties = from p in obj.GetType().GetProperties()
							 let value = p.GetValue(obj)
							 where value != null
							 select $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(value.ToString() ?? string.Empty)}";

			return string.Join("&", properties);

		}
	}
}
