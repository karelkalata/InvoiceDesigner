using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;

namespace InvoiceDesigner.Infrastructure.Repositories.Directories
{
	public class CustomerRepository : ABaseRepository<Customer>, ICustomerRepository
	{
		public CustomerRepository(DataContext context) : base(context) { }

	}
}
