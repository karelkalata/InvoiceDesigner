using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IBankService
	{
		Task<Bank?> GetByIdAsync(int id);

		Task<IReadOnlyCollection<BankAutocompleteDto>> GetAllBanksAutocompleteDto();

	}
}
