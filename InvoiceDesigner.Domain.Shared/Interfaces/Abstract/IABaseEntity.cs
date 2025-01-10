namespace InvoiceDesigner.Domain.Shared.Interfaces.Abstract
{
	public interface IABaseEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
	}
}
