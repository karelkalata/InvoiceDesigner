using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Directories
{
	public class Company : ABaseEntity
	{
		public string TaxId { get; set; } = string.Empty;
		public string? VatId { get; set; }
		public string? WWW { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Address { get; set; }
		public string? Info { get; set; }
		public int PaymentTerms { get; set; } = 14;
		public decimal DefaultVat { get; set; } = 21;
		public int CurrencyId { get; set; }
		public Currency Currency { get; set; } = null!;
		public ICollection<Bank> Banks { get; set; } = new List<Bank>();
	}
}
