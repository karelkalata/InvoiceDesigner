namespace InvoiceDesigner.Domain.Shared.Responses
{
	public class ResponsePaged<T> where T : class
	{
		public required IReadOnlyCollection<T> Items { get; set; }

		public int TotalCount { get; set; } = 0;
	}
}
