using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Application.QueryParameters
{
	public class QueryChangeProperty
	{
		public int EntityId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
		public EStatus Status { get; set; }
	}
}
