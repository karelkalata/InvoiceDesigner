using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.QueryParameters
{
	public class QueryFiltering
	{
		public int UserId { get; set; }
		public bool IsAdmin { get; set; } = false;
		public EAccountingDocument AccountingDocument { get; set; }
		public string? SearchString { get; set; } = string.Empty;
	}
}
