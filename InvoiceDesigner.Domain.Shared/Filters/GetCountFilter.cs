namespace InvoiceDesigner.Domain.Shared.Records
{
	public record GetCountFilter
	{
		public bool ShowDeleted { get; init; }
		public bool ShowArchived { get; init; }
	}
}
