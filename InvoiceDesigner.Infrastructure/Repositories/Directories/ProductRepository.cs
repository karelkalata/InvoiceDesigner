using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Directories
{
	public class ProductRepository : ABaseRepository<Product>, IProductRepository
	{
		public ProductRepository(DataContext context) : base(context){ }

		public override async Task<IReadOnlyCollection<Product>> GetEntitiesAsync(QueryPaged queryPaged, string sortLabel)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<Product> query = _dbSet.AsNoTracking();

			if (!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				query = query.Where(c => c.Name.ToLower().Contains(queryPaged.SearchString.ToLower()));
			}

			query = GetOrdering(sortLabel)(query);

			return await query
				.Include(a => a.ProductPrice)
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}

		public override async Task<Product?> GetByIdAsync(int id)
		{
			return await _dbSet
				.Include(a => a.ProductPrice)
					.ThenInclude(c => c.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}
 
	}
}
