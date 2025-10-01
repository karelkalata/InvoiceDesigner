using InvoiceDesigner.Domain.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Application.DTOs.BankReceiptDTOs
{
	public class BankReceiptCreateDto
	{
		public int Id { get; set; }
		public DateTime? DateTime { get; set; }
		[Required]
		public int InvoiceId { get; set; }
		public bool IsArchived { get; set; }
		public EStatus Status { get; set; }
	}
}
