using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;

namespace InvoiceDesigner.Application.Mapper.Accounting
{
	public class MapperChartOfAccounts : Profile
	{
		public MapperChartOfAccounts()
		{
			CreateMap<ChartOfAccounts, ChartOfAccountsDto>();
			CreateMap<ChartOfAccounts, ChartOfAccountsAutocompleteDto>();
		}
	}
}
