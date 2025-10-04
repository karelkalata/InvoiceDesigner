using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Filters
{
	public record PagedFilter
	{
		public int PageSize { get; init; }
		public int Page { get; init; }
		public bool ShowDeleted { get; init; } = false;
		public bool ShowArchived { get; init; } = false;
		public string? SearchString { get; init; } = string.Empty;
		public string? ExcludeString { get; init; } = string.Empty;
		public string SortLabel { get; init; } = string.Empty;
		public IReadOnlyCollection<Company>? UserAuthorizedCompanies { get; init; } = null;
	}
}
