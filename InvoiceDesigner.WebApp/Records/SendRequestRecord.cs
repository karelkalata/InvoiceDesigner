namespace InvoiceDesigner.WebApp.Records
{
	public record RecordSendPost<T>
	{
		public string Url { get; init; } = string.Empty;
		public required T ModelSend { get; init; }
		public bool IsUpdate { get; init; } = false;
		public string RedirectUrl { get; init; } = string.Empty;
	}
}
