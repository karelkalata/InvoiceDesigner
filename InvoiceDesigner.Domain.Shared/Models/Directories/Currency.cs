using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Directories
{
	public class Currency : ABaseEntity
	{
		public string Description { get; set; } = string.Empty;
	}
}
