using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly DataContext _context;

		public CustomerRepository(DataContext context)
		{
			_context = context;
		}


		public async Task<IReadOnlyCollection<Customer>> GetCustomersAsync(QueryPaged queryPaged, 
																			Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<Customer> query = _context.Customers.AsNoTracking();

			if (!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				query = query.Where(c => c.Name.ToLower().Contains(queryPaged.SearchString.ToLower()));
			}

			query = orderBy(query);

			return await query
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}


		public async Task<int> CreateCustomerAsync(Customer entity)
		{
			await _context.Customers.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<Customer?> GetCustomerByIdAsync(int id)
		{
			return await _context.Customers
				.FirstOrDefaultAsync(c => c.Id == id);
		}


		public async Task<int> UpdateCustomerAsync(Customer entity)
		{
			_context.Customers.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<bool> DeleteCustomerAsync(Customer entity)
		{
			_context.Customers.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}


		public async Task<int> GetCountCustomersAsync(bool showDeleted)
		{
			IQueryable<Customer> query = _context.Customers;

			if (!showDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			return await query.CountAsync();
		}

	}
}
