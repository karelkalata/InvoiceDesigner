namespace InvoiceDesigner.Domain.Shared.Filters
{
	public record GetCountFilter
	{
		public bool ShowDeleted { get; init; }
		public bool ShowArchived { get; init; }
	}
}
