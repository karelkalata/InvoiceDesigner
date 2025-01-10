using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;

namespace InvoiceDesigner.Application.Services
{
	public class CompanyServiceHelper : ICompanyServiceHelper
	{
		private readonly ICompanyRepository _repository;

		public CompanyServiceHelper(ICompanyRepository repository)
		{
			_repository = repository;
		}

		public async Task<bool> IsCurrencyUsedInCompany(int currencyId)
		{
			return await _repository.IsCurrencyUsedInCompany(currencyId);
		}

	}
}
