namespace InvoiceDesigner.Domain.Shared.QueryParameters.Report
{
	public class QueryTrialBalance
	{
		public int UserId { get; set; }
		public bool IsAdmin { get; set; }
		public int PageSize { get; set; }
		public int Page { get; set; }
		public DateTime? DateStart { get; set; }
		public DateTime? DateEnd { get; set; }
		public List<int> CompaniesIDs { get; set; } = new List<int>();
	}
}
