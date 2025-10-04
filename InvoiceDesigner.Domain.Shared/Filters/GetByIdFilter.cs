using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Filters
{
	public record GetByIdFilter
	{
		public int Id { get; init; }
		public IReadOnlyCollection<Company> userAuthorizedCompanies { get; init; } = new List<Company>();
	}
}
