﻿
using AutoMapper;
using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Product;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Records;

namespace InvoiceDesigner.Application.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _repoProduct;
		private readonly IMapper _mapper;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;
		private readonly ICurrencyService _currencyService;

		public ProductService(IProductRepository repoProduct,
								IMapper mapper,
								IInvoiceServiceHelper invoiceServiceHelper,
								ICurrencyService currencyService)
		{
			_repoProduct = repoProduct;
			_mapper = mapper;
			_invoiceServiceHelper = invoiceServiceHelper;
			_currencyService = currencyService;
		}

		public async Task<ResponsePaged<ProductsViewDto>> GetPagedAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var productsTask = _repoProduct.GetEntitiesAsync(queryPaged, queryPaged.SortLabel);

			var recordGetCount = new GetCountFilter
			{
				ShowArchived = queryPaged.ShowArchived,
				ShowDeleted = queryPaged.ShowDeleted,
			};
			var totalCountTask = _repoProduct.GetCountAsync(recordGetCount);

			await Task.WhenAll(productsTask, totalCountTask);

			var productsDto = _mapper.Map<IReadOnlyCollection<ProductsViewDto>>(await productsTask);
			var result = new ResponsePaged<ProductsViewDto>
			{
				Items = productsDto,
				TotalCount = await totalCountTask
			};

			return result;
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, ProductEditDto productEditDto)
		{
			var existsProduct = new Product();
			await MapProduct(existsProduct, productEditDto);

			var entityId = await _repoProduct.CreateAsync(existsProduct);

			return new ResponseRedirect
			{
				RedirectUrl = "/Products"
			};
		}

		public Task<Product> GetByIdAsync(int id) => ValidateExistsEntityAsync(id);

		public async Task<ProductEditDto> GetEditDtoByIdAsync(int id)
		{
			var existsProduct = await ValidateExistsEntityAsync(id);
			return _mapper.Map<ProductEditDto>(existsProduct);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, ProductEditDto productEditDto)
		{
			var existsProduct = await ValidateExistsEntityAsync(productEditDto.Id);
			await MapProduct(existsProduct, productEditDto);

			await _repoProduct.UpdateAsync(existsProduct);

			return new ResponseRedirect
			{
				RedirectUrl = "/Products"
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand)
		{
			var existsEntity = await ValidateExistsEntityAsync(deleteEntityCommand.EntityId);

			if (!deleteEntityCommand.MarkAsDeleted)
			{
				if (await _invoiceServiceHelper.IsProductUsedInInvoiceItems(deleteEntityCommand.EntityId))
					throw new InvalidOperationException($"{existsEntity.Name} is in use in Invoices and cannot be deleted.");

				return new ResponseBoolean
				{
					Result = await _repoProduct.DeleteAsync(existsEntity)
				};
			}
			else
			{
				existsEntity.IsDeleted = true;
				await _repoProduct.UpdateAsync(existsEntity);

				return new ResponseBoolean
				{
					Result = true
				};
			}
		}

		public Task<int> GetCountAsync() => _repoProduct.GetCountAsync(new GetCountFilter());

		public async Task<IReadOnlyCollection<ProductAutocompleteDto>> FilteringData(string searchText)
		{
			var queryPaged = new QueryPaged
			{
				PageSize = 10,
				Page = 1,
				SearchString = searchText
			};

			var products = await _repoProduct.GetEntitiesAsync(queryPaged, "Name");

			return _mapper.Map<IReadOnlyCollection<ProductAutocompleteDto>>(products);
		}

		private async Task<Product> ValidateExistsEntityAsync(int id)
		{
			var existsProduct = await _repoProduct.GetByIdAsync(id)
				?? throw new InvalidOperationException("Item not found");
			return existsProduct;
		}

		private async Task MapProduct(Product existsProduct, ProductEditDto dto)
		{
			existsProduct.Name = dto.Name.Trim();

			List<ProductPrice> newProductPrices = new List<ProductPrice>();

			foreach (var productPrice in dto.ProductPrice)
			{
				var currency = await _currencyService.GetByIdAsync(productPrice.Currency.Id)
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
