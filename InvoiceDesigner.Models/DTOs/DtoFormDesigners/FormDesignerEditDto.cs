﻿namespace InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners
{
	public class FormDesignerEditDto
	{
		public int Id { get; init; }

		public string Name { get; set; } = string.Empty;

		public List<FormDesignerSchemeEditDto> Schemes { get; set; } = new List<FormDesignerSchemeEditDto>();

		public List<DropItemEditDto> DropItems { get; set; } = new List<DropItemEditDto>();

	}
}
