namespace InvoiceDesigner.Domain.Shared.QueryParameters
{
	public class QueryDeleteEntity
	{
		public int UserId { get; set; }

		public bool IsAdmin { get; set; } = false;

		public int EntityId { get; set; }

		public bool MarkAsDeleted { get; set; } = false;
	}
}
