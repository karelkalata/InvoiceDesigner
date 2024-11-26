using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IBankRepository
	{

		Task<IReadOnlyCollection<Bank>> GetAllBanksAsync();

		Task<Bank?> GetBankByIdAsync(int id);

		Task<bool> IsCurrencyUsedInBanksAsync(int currencyId);

	}
}
