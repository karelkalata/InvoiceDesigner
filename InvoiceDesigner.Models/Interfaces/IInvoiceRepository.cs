using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IInvoiceRepository
	{
		Task<IReadOnlyCollection<Invoice>> GetInvoicesAsync(int pageSize,
											int pageNumber,
											string searchString,
											Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy);

		Task<int> CreateInvoiceAsync(Invoice entity);

		Task<Invoice?> GetInvoiceByIdAsync(int id);

		Task<int> UpdateInvoiceAsync(Invoice entity);

		Task<bool> DeleteInvoiceAsync(Invoice entity);

		Task<int> GetCountInvoicesAsync();

		Task<int> GetNextInvoiceNumberForCompanyAsync(int companyId);

		Task<bool> IsCompanyUsedInInvoices(int companyId);

		Task<bool> IsBankUsedInInvoices(int bankId);

		Task<bool> IsClientUsedInInvoices(int clientId);

		Task<bool> IsCurrencyUsedInInvoices(int currencyId);

		Task<bool> IsProductUsedInInvoiceItems(int productId);
	}
}
