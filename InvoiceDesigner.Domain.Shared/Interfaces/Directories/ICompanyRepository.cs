using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Directories
{
	public interface ICompanyRepository : IABaseRepository<Company>
	{
		Task<IReadOnlyCollection<Company>> GetAllCompanies();
		Task<bool> IsCurrencyUsedI(int currencyId);
	}
}
