using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Directories
{
	public class CurrencyRepository : ABaseRepository<Currency>, ICurrencyRepository
	{
		public CurrencyRepository(DataContext context) : base(context) { }

		public async Task<IReadOnlyCollection<Currency>> GetAllAsync()
		{
			return await _dbSet
				.AsNoTracking()
				.ToListAsync();
		}

	}
}
