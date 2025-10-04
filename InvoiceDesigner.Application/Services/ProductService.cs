using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Product;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Mapper;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Application.Services.Abstract;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Services
{
	public class ProductService : ABaseService<Product>, IProductService
	{
		private readonly IProductRepository _repository;
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly ICurrencyRepository _repositoryCurrency;

		public ProductService(IProductRepository repository, IInvoiceRepository invoiceRepository, ICurrencyRepository repositoryCurrency) : base(repository)
		{
			_repository = repository;
			_invoiceRepository = invoiceRepository;
			_repositoryCurrency = repositoryCurrency;
		}

		public async Task<ResponsePaged<ProductsViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand)
		{
			var (entities, total) = await GetEntitiesAndCountAsync(pagedCommand);
			var dtos = ProductMapper.ToViewDto(entities);

			return new ResponsePaged<ProductsViewDto>
			{
				Items = dtos,
				TotalCount = total
			};
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, ProductEditDto productEditDto)
		{
			var existsProduct = new Product();
			await MapProduct(existsProduct, productEditDto);
			await _repository.CreateAsync(existsProduct);

			return new ResponseRedirect
			{
				RedirectUrl = "/Products"
			};
		}

		public Task<Product> GetByIdAsync(int id) => ValidateExistsEntityAsync(id);

		public async Task<ProductEditDto> GetEditDtoByIdAsync(int id)
		{
			var existsProduct = await ValidateExistsEntityAsync(id);
			return ProductMapper.ToEditDto(existsProduct);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, ProductEditDto productEditDto)
		{
			var existsProduct = await ValidateExistsEntityAsync(productEditDto.Id);
			await MapProduct(existsProduct, productEditDto);

			await _repository.UpdateAsync(existsProduct);

			return new ResponseRedirect
			{
				RedirectUrl = "/Products"
			};
		}

		public override async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand)
		{
			var existsEntity = await ValidateExistsEntityAsync(deleteEntityCommand.EntityId);
			return await base.DeleteOrMarkAsDeletedAsync(deleteEntityCommand);
		}

		public Task<int> GetCountAsync() => _repository.GetCountAsync(new GetCountFilter());

		public async Task<IReadOnlyCollection<ProductAutocompleteDto>> FilteringData(string searchText)
		{
			var pagedFilter = new PagedFilter
			{
				PageSize = 10,
				Page = 1,
				SearchString = searchText,
				SortLabel = "Name",
			};

			var products = await _repository.GetEntitiesAsync(pagedFilter);

			return ProductMapper.ToAutocompleteDto(products);
		}

		private async Task<Product> ValidateExistsEntityAsync(int id)
		{
			var existsProduct = await _repository.GetByIdAsync(new GetByIdFilter { Id = id })
				?? throw new InvalidOperationException("Item not found");
			return existsProduct;
		}

		private async Task MapProduct(Product existsProduct, ProductEditDto dto)
		{
			existsProduct.Name = dto.Name.Trim();

			List<ProductPrice> newProductPrices = new List<ProductPrice>();

			foreach (var productPrice in dto.ProductPrice)
			{
				var currency = await _repositoryCurrency.GetByIdAsync(new GetByIdFilter { Id = productPrice.Currency.Id })
								?? throw new InvalidOperationException($"Currency with ID {productPrice.Id} not found.");

				newProductPrices.Add(new ProductPrice
				{
					ProductId = existsProduct.Id,
					CurrencyId = currency.Id,
					Currency = currency,
					Price = productPrice.Price,
				});
			}
			existsProduct.ProductPrice = newProductPrices;
		}

	}
}
