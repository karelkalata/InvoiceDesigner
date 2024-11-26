using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class FormDesignersRepository : IFormDesignersRepository
	{
		private readonly DataContext _context;

		public FormDesignersRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<FormDesigner>> GetAllFormDesignersAsync()
		{
			return await _context.FormDesigners
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<int> CreateFormDesignerAsync(FormDesigner entity)
		{
			await _context.FormDesigners.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<FormDesigner?> GetFormDesignerByIdAsync(int id)
		{
			return await _context.FormDesigners
				.Include(a => a.Schemes)
				.Include(b => b.DropItems)
					.ThenInclude(c => c.CssStyle)
				.FirstOrDefaultAsync(e => e.Id == id);
		}


		public async Task<int> UpdateFormDesignerAsync(FormDesigner entity)
		{
			_context.FormDesigners.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<bool> DeleteFormDesignerAsync(FormDesigner entity)
		{
			_context.FormDesigners.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}


		public void DeleteDropItemsFromContext(DropItem dropItem)
		{
			_context.DropItems.Remove(dropItem);
		}

	}
}
