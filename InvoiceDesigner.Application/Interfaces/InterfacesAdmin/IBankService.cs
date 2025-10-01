using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Interfaces.Admin
{
	public interface IBankService
	{
		Task<Bank?> GetByIdAsync(int id);

		Task<IReadOnlyCollection<BankAutocompleteDto>> GetAllBanksAutocompleteDto();

	}
}
