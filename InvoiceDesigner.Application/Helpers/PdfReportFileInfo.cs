namespace InvoiceDesigner.Application.Helpers
{
	public class PdfReportFileInfo
	{
		public byte[] ByteArray { get; set; } = null!;
		public string MimeType { get; set; } = null!;
		public string FileName { get; set; } = null!;
	}
}
