using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.Interfaces;

namespace InvoiceDesigner.Application.Services
{
	public class ProductServiceHelper : IProductServiceHelper
	{
		private readonly IProductRepository _repository;

		public ProductServiceHelper(IProductRepository repository)
		{
			_repository = repository;
		}

		public Task<bool> IsCurrencyUsedInProduct(int currencyId)
		{
			return _repository.IsCurrencyUsedInProduct(currencyId);
		}
	}
}
