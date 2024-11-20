using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
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


		public async Task<IReadOnlyCollection<Customer>> GetCustomersAsync(int pageSize, int page, string searchString, Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy, bool showDeleted = false)
		{
			int skip = (page - 1) * pageSize;

			IQueryable<Customer> query = _context.Customers.AsNoTracking();

			if (!showDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(searchString))
			{
				query = query.Where(c => c.Name.ToLower().Contains(searchString.ToLower()));
			}

			query = orderBy(query);

			return await query
				.Skip(skip)
				.Take(pageSize)
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
