using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Directories
{
	public interface IBankRepository : IABaseRepository<Bank>
	{
		Task<IReadOnlyCollection<Bank>> GetAllAsync();
		Task<bool> IsCurrencyUsedInBanksAsync(int currencyId);
	}
}
