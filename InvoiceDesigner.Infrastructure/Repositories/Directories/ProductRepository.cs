using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Directories
{
	public class ProductRepository : ABaseRepository<Product>, IProductRepository
	{
		public ProductRepository(DataContext context) : base(context) { }

		public override async Task<IReadOnlyCollection<Product>> GetEntitiesAsync(PagedFilter pagedFilter)
		{
			int skip = (pagedFilter.Page - 1) * pagedFilter.PageSize;

			IQueryable<Product> query = _dbSet.AsNoTracking();

			if (!pagedFilter.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(pagedFilter.SearchString))
			{
				query = query.Where(c => c.Name.ToLower().Contains(pagedFilter.SearchString.ToLower()));
			}

			if (!string.IsNullOrEmpty(pagedFilter.SortLabel))
			{
				query = GetOrdering(pagedFilter.SortLabel)(query);
			}

			return await query
				.Include(a => a.ProductPrice)
				.Skip(skip)
				.Take(pagedFilter.PageSize)
				.ToListAsync();
		}

		public override async Task<Product?> GetByIdAsync(GetByIdFilter getByIdFilter)
		{
			return await _dbSet
				.Include(a => a.ProductPrice)
					.ThenInclude(c => c.Currency)
				.Where(c => c.Id == getByIdFilter.Id)
				.SingleOrDefaultAsync();
		}

	}
}
