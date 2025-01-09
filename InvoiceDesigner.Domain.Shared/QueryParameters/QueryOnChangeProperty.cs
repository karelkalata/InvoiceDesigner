using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.QueryParameters
{
	public class QueryOnChangeProperty
	{
		public int UserId { get; set; }

		public bool IsAdmin { get; set; } = false;

		public int EntityId { get; set; }

		public bool IsDeleted { get; set; }

		public bool IsArchived { get; set; }

		public EStatus Status { get; set; }
	}
}
