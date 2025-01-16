namespace InvoiceDesigner.Domain.Shared.Interfaces.Abstract
{
	public interface IHasIdAndName
	{
		int Id { get; set; }
		string Name { get; set; }
	}
}
