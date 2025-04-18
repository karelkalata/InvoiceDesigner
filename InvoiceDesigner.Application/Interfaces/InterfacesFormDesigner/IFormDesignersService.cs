﻿using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner
{
	public interface IFormDesignersService
	{
		Task<IReadOnlyCollection<FormDesignersAutocompleteDto>> GetAllAutocompleteDto(EAccountingDocument typeDocument);

		Task<ResponseRedirect> CreateAsync(int userId, FormDesignerEditDto formDesignerEditDto);

		Task<FormDesigner> GetByIdAsync(int id);

		Task<FormDesignerEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(int userId, FormDesignerEditDto formDesignerEditDto);

		Task<ResponseBoolean> DeleteAsync(int userId, int id);

		DropItemEditDto AddEmptyBox();

		Task<IReadOnlyCollection<FormDesignersAutocompleteDto>> FilteringData(QueryFiltering queryFilter);
	}
}
