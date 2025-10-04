using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Application.QueryParameters
{
	public class QueryUserLogin
	{
		[Required]
		public string Login { get; set; } = string.Empty;
		[Required]
		public string Password { get; set; } = string.Empty;
	}
}
