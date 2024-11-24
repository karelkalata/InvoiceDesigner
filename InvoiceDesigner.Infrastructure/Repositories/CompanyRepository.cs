using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class CompanyRepository : ICompanyRepository
	{
		private readonly DataContext _context;

		public CompanyRepository(DataContext context)
		{
			_context = context;
		}


		public async Task<IReadOnlyCollection<Company>> GetCompaniesAsync(QueryPaged queryPaged, Func<IQueryable<Company>, IOrderedQueryable<Company>> orderBy)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<Company> query = _context.Companies.AsNoTracking();

			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				var searchString = queryPaged.SearchString.ToLower();
				query = query.Where(c => c.Name.ToLower().Contains(searchString) || c.TaxId.ToLower().Contains(searchString));
			}

			query = orderBy(query);

			return await query
				.Include(a => a.Currency)
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}


		public async Task<int> CreateCompanyAsync(Company entity)
		{
			await _context.Companies.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<Company?> GetCompanyByIdAsync(int id)
		{
			return await _context.Companies
				.Include(c => c.Currency)
				.Include(c => c.Banks)
					.ThenInclude(c => c.Currency)
				.FirstOrDefaultAsync(c => c.Id == id);
		}


		public async Task<int> UpdateCompanyAsync(Company entity)
		{
			_context.Companies.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<bool> DeleteCompanyAsync(Company entity)
		{
			_context.Companies.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}


		public async Task<int> GetCountCompaniesAsync()
		{
			return await _context.Companies.CountAsync();
		}


		public async Task<IReadOnlyCollection<Company>> GetAllCompaniesDto()
		{
			return await _context.Companies
				.Include(a => a.Currency)
				.Include(b => b.Banks)
					.ThenInclude(c => c.Currency)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<bool> IsCurrencyUsedInCompany(int currencyId)
		{
			return await _context.Companies.AnyAsync(c => c.CurrencyId == currencyId);
		}
	}
}
