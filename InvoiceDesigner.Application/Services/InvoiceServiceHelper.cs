using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.Interfaces;

namespace InvoiceDesigner.Application.Services
{
	public class InvoiceServiceHelper : IInvoiceServiceHelper
	{
		private readonly IInvoiceRepository _repository;

		public InvoiceServiceHelper(IInvoiceRepository repository)
		{
			_repository = repository;
		}

		public async Task<bool> IsCompanyUsedInInvoices(int companyId)
		{
			return await _repository.IsCompanyUsedInInvoices(companyId);
		}

		public async Task<bool> IsBankUsedInInvoices(int bankId)
		{
			return await _repository.IsBankUsedInInvoices(bankId);
		}

		public async Task<bool> IsClientUsedInInvoices(int clientId)
		{
			return await _repository.IsClientUsedInInvoices(clientId);
		}

		public async Task<bool> IsCurrencyUsedInInvoices(int currencyId)
		{
			return await _repository.IsCurrencyUsedInInvoices(currencyId);
		}

		public async Task<bool> IsProductUsedInInvoiceItems(int productId)
		{
			return await _repository.IsProductUsedInInvoiceItems(productId);
		}

	}
}
