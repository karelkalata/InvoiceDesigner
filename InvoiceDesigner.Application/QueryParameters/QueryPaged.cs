﻿namespace InvoiceDesigner.Application.QueryParameters
{
	public class QueryPaged
	{
		public int PageSize { get; set; }
		public int Page { get; set; }
		public string? SearchString { get; set; } = string.Empty;
		public string SortLabel { get; set; } = string.Empty;
		public bool ShowDeleted { get; set; } = false;
		public bool ShowArchived { get; set; } = false;
	}
}
