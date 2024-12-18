namespace InvoiceDesigner.Domain.Shared.Responses
{
	public class ResponseJwtToken
	{
		public string JwtToken { get; set; } = string.Empty;

		public string Locale { get; set; } = "en-US";
	}

}
