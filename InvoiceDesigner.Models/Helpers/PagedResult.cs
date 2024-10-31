namespace InvoiceDesigner.Domain.Shared.Helpers
{
    public class PagedResult<T> where T : class
    {
        public required IReadOnlyCollection<T> Items { get; set; }
        public int TotalCount { get; set; } = 0;
    }
}
