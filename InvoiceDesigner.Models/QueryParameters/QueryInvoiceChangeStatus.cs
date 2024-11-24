using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.QueryParameters
{
	public class QueryInvoiceChangeStatus
	{
		public int UserId { get; set; }
		public bool IsAdmin { get; set; } = false;
		public int EntityId { get; set; }
		public EInvoiceStatus Status { get; set; }
	}
}
