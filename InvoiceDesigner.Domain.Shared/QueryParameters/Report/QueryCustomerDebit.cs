namespace InvoiceDesigner.Domain.Shared.QueryParameters.Report
{
	public class QueryCustomerDebit
	{
		public int UserId { get; set; }
		public bool IsAdmin { get; set; }
		public List<int> CompaniesIDs { get; set; } = new List<int>();
		public int CustomerId { get; set; }
	}
}
