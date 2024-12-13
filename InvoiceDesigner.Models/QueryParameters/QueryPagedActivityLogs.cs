using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.QueryParameters
{
	public class QueryPagedActivityLogs
	{
		public int Page { get; set; }

		public int PageSize { get; set; }

		public EDocumentsTypes? DocumentTypes { get; set; }

		public int? EntityId { get; set; }

		public string? EntityNumber { get; set; }

	}
}
