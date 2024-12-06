using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
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

		public async Task<IReadOnlyCollection<Product>> GetEntitiesAsync(QueryPaged queryPaged,
																		Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<Product> query = _context.Products.AsNoTracking();

			if(!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				query = query.Where(c => c.Name.ToLower().Contains(queryPaged.SearchString.ToLower()));
			}

			query = orderBy(query);

			return await query
				.Include(a => a.ProductPrice)
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}


		public async Task<int> CreateAsync(Product entity)
		{
			await _context.Products.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<Product?> GetByIdAsync(int id)
		{
			return await _context.Products
				.Include(a => a.ProductPrice)
					.ThenInclude(c => c.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}


		public async Task<int> UpdateAsync(Product entity)
		{
			_context.Products.Update(entity);
			await _context.SaveChangesAsync()
				;
			return entity.Id;
		}


		public async Task<bool> DeleteAsync(Product entity)
		{
			_context.Products.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountAsync(bool showDeleted)
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
