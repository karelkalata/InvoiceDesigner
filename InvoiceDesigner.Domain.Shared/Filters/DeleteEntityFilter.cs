namespace InvoiceDesigner.Domain.Shared.Filters
{
	public record DeleteEntityFilter
	{
		public int EntityId { get; init; }
	}
}
