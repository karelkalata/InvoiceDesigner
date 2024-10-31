﻿namespace InvoiceDesigner.Domain.Shared.Models
{
	public class Product
	{
		public int Id { get; init; }

		public string Name { get; set; } = string.Empty;

		public ICollection<ProductPrice> ProductPrice { get; set; } = new List<ProductPrice>();

	}
}