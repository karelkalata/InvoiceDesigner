namespace InvoiceDesigner.Domain.Shared.QueryParameters
{
	public class QueryInvoiceChangeArchive
	{
		public int UserId { get; set; }
		public bool IsAdmin { get; set; } = false;
		public int EntityId { get; set; }
		public bool Archive { get; set; }
	}
}
