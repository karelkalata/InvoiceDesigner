using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Mapper;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Services
{
	public class BankService : IBankService
	{
		private readonly IBankRepository _repository;

		public BankService(IBankRepository repository)
		{
			_repository = repository;
		}

		public async Task<Bank?> GetByIdAsync(int id)
		{
			return await _repository.GetByIdAsync(new GetByIdFilter { Id = id });
		}

		public async Task<IReadOnlyCollection<BankAutocompleteDto>> GetAllBanksAutocompleteDto()
		{
			var banks = await _repository.GetAllAsync();
			return banks.Select(BankMapper.ToAutocompleteDto).ToList();
		}

	}
}
