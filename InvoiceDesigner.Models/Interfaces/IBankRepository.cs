using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IBankRepository
	{

		Task<IReadOnlyCollection<Bank>> GetAllBanksAsync();

		Task<IReadOnlyCollection<Bank>> GetBanksAsync(int pageSize,
														int pageNumber,
														string searchString,
														Func<IQueryable<Bank>, IOrderedQueryable<Bank>> orderBy);
		Task<int> CreateBankAsync(Bank entity);

		Task<Bank?> GetBankByIdAsync(int id);

		Task<int> UpdateBankAsync(Bank entity);

		Task<bool> DeleteBankAsync(Bank entity);

		Task<int> GetCountBanksAsync();

		Task<bool> IsCurrencyUsedInBanksAsync(int currencyId);
	}
}
