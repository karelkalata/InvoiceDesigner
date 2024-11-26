namespace InvoiceDesigner.Domain.Shared.Responses
{
	public class ResponsePdf
	{
		public byte[] ByteArray { get; set; } = null!;

		public string MimeType { get; set; } = null!;

		public string FileName { get; set; } = null!;

	}
}
