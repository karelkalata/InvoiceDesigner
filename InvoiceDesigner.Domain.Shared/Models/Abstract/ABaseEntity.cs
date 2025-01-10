using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Abstract
{
	public abstract class ABaseEntity : IABaseEntity
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
	}
}
