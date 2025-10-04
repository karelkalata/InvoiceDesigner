
namespace InvoiceDesigner.Application.Commands
{
	public record PagedCommand
	{
		public int UserId { get; init; }
		public bool IsAdmin { get; init; } = false;
		public int PageSize { get; init; }
		public int Page { get; init; }
		public bool ShowDeleted { get; init; } = false;
		public bool ShowArchived { get; init; } = false;
		public string? SearchString { get; init; }
		public string SortLabel { get; init; } = string.Empty;
	}
}
