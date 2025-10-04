using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Directories
{
	public class UserRepository : ABaseRepository<User>, IUserRepository
	{
		public UserRepository(DataContext context) : base(context) { }

		public override async Task<User?> GetByIdAsync(GetByIdFilter getByIdFilter)
		{
			return await _dbSet
				.Where(u => u.Id == getByIdFilter.Id)
				.Include(u => u.Companies)
					.ThenInclude(company => company.Currency)
				.Include(u => u.Companies)
					.ThenInclude(company => company.Banks)
						.ThenInclude(bank => bank.Currency)
				.SingleOrDefaultAsync();
		}

		public async Task<User?> GetByLoginAsync(string loginName)
		{
			return await _dbSet
				.Where(c => c.Login == loginName)
				.SingleOrDefaultAsync();
		}

	}
}
