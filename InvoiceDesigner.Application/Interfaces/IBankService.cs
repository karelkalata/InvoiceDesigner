using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IBankService
	{
		Task<PagedResult<BankViewDto>> GetPagedBanksAsync(int pageSize, int page, string searchString, string sortLabel);

		Task<ResponseRedirect> CreateBankAsync(BankEditDto bankEditDto);

		Task<Bank?> GetBankByIdAsync(int id);

		Task<BankEditDto> GetBankEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateBankAsync(BankEditDto bankEditDto);

		Task<bool> DeleteBankAsync(int id);

		Task<int> GetBanksCountAsync();

		Task<IReadOnlyCollection<BankAutocompleteDto>> GetAllBanksAutocompleteDto();
	}
}
