using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly DataContext _context;

		public ProductRepository(DataContext context)
		{
			_context = context;
		}


		public async Task<IReadOnlyCollection<Product>> GetProductsAsync(	int pageSize, 
																			int page, 
																			string searchString, 
																			Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy,
																			bool showDeleted = false)
		{
			int skip = (page - 1) * pageSize;

			IQueryable<Product> query = _context.Products.AsNoTracking();

			if(!showDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(searchString))
			{
				query = query.Where(c => c.Name.ToLower().Contains(searchString.ToLower()));
			}

			query = orderBy(query);

			return await query
				.Include(a => a.ProductPrice)
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();
		}


		public async Task<int> CreateProductAsync(Product entity)
		{
			await _context.Products.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<Product?> GetProductByIdAsync(int id)
		{
			return await _context.Products
				.Include(a => a.ProductPrice)
					.ThenInclude(c => c.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}


		public async Task<int> UpdateProductAsync(Product entity)
		{
			_context.Products.Update(entity);
			await _context.SaveChangesAsync()
				;
			return entity.Id;
		}


		public async Task<bool> DeleteProductAsync(Product entity)
		{
			_context.Products.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountProductsAsync(bool showDeleted)
		{
			IQueryable<Product> query = _context.Products;

			if (!showDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			return await query.CountAsync();
		}

		public async Task<bool> IsCurrencyUsedInProduct(int currencyId)
		{
			return await _context.ProductPrice.AnyAsync(c => c.CurrencyId == currencyId);
		}
	}
}
