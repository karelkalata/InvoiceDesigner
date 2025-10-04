using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Directories
{
	public class CompanyRepository : ABaseRepository<Company>, ICompanyRepository
	{
		public CompanyRepository(DataContext context) : base(context) { }

		public override async Task<IReadOnlyCollection<Company>> GetEntitiesAsync(PagedFilter pagedFilter)
		{
			int skip = (pagedFilter.Page - 1) * pagedFilter.PageSize;

			IQueryable<Company> query = _dbSet.AsNoTracking();

			if (!pagedFilter.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(pagedFilter.SearchString))
			{
				var searchString = pagedFilter.SearchString.ToLower();
				query = query.Where(c => c.Name.ToLower().Contains(searchString));
			}

			if (!string.IsNullOrEmpty(pagedFilter.ExcludeString))
			{
				var excludeString = pagedFilter.ExcludeString.ToLower();
				query = query.Where(c => !c.Name.ToLower().Contains(excludeString));
			}

			if (!string.IsNullOrEmpty(pagedFilter.SortLabel))
			{
				query = GetOrdering(pagedFilter.SortLabel)(query);
			}

			return await query
				.Include(c => c.Currency)
				.Skip(skip)
				.Take(pagedFilter.PageSize)
				.ToListAsync();
		}

		public override async Task<Company?> GetByIdAsync(GetByIdFilter getByIdFilter)
		{
			return await _dbSet
				.Include(c => c.Currency)
				.Include(c => c.Banks)
					.ThenInclude(c => c.Currency)
				.FirstOrDefaultAsync(c => c.Id == getByIdFilter.Id);
		}

		public async Task<IReadOnlyCollection<Company>> GetAllCompanies()
		{
			return await _dbSet
				.Include(a => a.Currency)
				.Include(b => b.Banks)
					.ThenInclude(c => c.Currency)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<bool> IsCurrencyUsedI(int currencyId)
		{
			return await _dbSet
				.AnyAsync(c => c.CurrencyId == currencyId);
		}
	}
}
