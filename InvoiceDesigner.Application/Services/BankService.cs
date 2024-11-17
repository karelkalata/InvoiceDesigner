using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Services
{
	public class BankService : IBankService
	{
		private readonly IBankRepository _repository;
		private readonly IMapper _mapper;

		public BankService(IBankRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Bank?> GetBankByIdAsync(int id)
		{
			return await _repository.GetBankByIdAsync(id);
		}

		public async Task<IReadOnlyCollection<BankAutocompleteDto>> GetAllBanksAutocompleteDto()
		{
			var banks = await _repository.GetAllBanksAsync();
			return _mapper.Map<IReadOnlyCollection<BankAutocompleteDto>>(banks);
		}
	}
}
