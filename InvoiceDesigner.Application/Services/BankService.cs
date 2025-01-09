using AutoMapper;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models.Directories;

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

		public async Task<Bank?> GetByIdAsync(int id)
		{
			return await _repository.GetByIdAsync(id);
		}

		public async Task<IReadOnlyCollection<BankAutocompleteDto>> GetAllBanksAutocompleteDto()
		{
			var banks = await _repository.GetAllAsync();
			return _mapper.Map<IReadOnlyCollection<BankAutocompleteDto>>(banks);
		}

	}
}
