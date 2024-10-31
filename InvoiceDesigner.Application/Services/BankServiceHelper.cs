using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.Interfaces;

namespace InvoiceDesigner.Application.Services
{
	public class BankServiceHelper : IBankServiceHelper
	{
		private readonly IBankRepository _repository;

		public BankServiceHelper(IBankRepository repository)
		{
			_repository = repository;
		}

		public async Task<bool> IsCurrencyUsedInBanks(int currencuId)
		{
			return await _repository.IsCurrencyUsedInBanksAsync(currencuId);
		}
	}
}
