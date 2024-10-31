namespace InvoiceDesigner.Domain.Shared.Helpers
{
	public class PdfFileInfo
	{
		public byte[] ByteArray { get; set; } = null!;

		public string MimeType { get; set; } = string.Empty;

		public string FileName { get; set; } = string.Empty;
	}
}
