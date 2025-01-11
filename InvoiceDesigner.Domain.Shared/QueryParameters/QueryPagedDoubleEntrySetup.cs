using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.QueryParameters
{
	public class QueryPagedDoubleEntrySetup
	{
		public int PageSize { get; set; }
		public int Page { get; set; }
		public EAccountingDocument AccountingDocument { get; set; }
	}
}
