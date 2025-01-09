using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Documents
{
	public interface IBankReceiptRepository
	{
		Task<IReadOnlyCollection<BankReceipt>> GetAsync(QueryPaged queryPaged,
									Func<IQueryable<BankReceipt>, IOrderedQueryable<BankReceipt>> orderBy,
									IReadOnlyCollection<Company> userAuthorizedCompanies);
		Task<int> CreateAsync(BankReceipt entity);
		Task<BankReceipt?> GetByIdAsync(int id, IReadOnlyCollection<Company> userAuthorizedCompanies);
		Task<BankReceipt?> GetByInvoiceIdAsync(int invoiceId, IReadOnlyCollection<Company> userAuthorizedCompanies);
		Task<int> UpdateAsync(BankReceipt entity);
		Task<bool> DeleteAsync(BankReceipt entity);
		Task<int> GetCountAsync(QueryPaged queryPaged, IReadOnlyCollection<Company> userAuthorizedCompanies);
		Task<int> GetNextNumberForCompanyAsync(int companyId);
	}
}
