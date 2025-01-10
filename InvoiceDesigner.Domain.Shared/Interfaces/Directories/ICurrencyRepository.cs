using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Directories
{
	public interface ICurrencyRepository : IABaseRepository<Currency>
	{
		Task<IReadOnlyCollection<Currency>> GetAllAsync();
	}
}
