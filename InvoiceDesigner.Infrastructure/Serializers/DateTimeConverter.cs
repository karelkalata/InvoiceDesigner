using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InvoiceDesigner.Infrastructure.Serializers
{
	//DateTimeConverter ISO 8601
	public class DateTimeConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return DateTime.Parse(reader.GetString()!, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
		}
	}
}
