﻿using AutoMapper;
using InvoiceDesigner.Application.DTOs.InvoiceDTOs;
using InvoiceDesigner.Domain.Shared.Models.Documents;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperInvoice : Profile
	{
		public MapperInvoice()
		{
			CreateMap<Invoice, InvoicePrintDto>();

			CreateMap<Invoice, InvoiceEditDto>()
				.ForMember(
					dest => dest.InvoiceItems,
					opt => opt.MapFrom(src => src.InvoiceItems)
				);

			CreateMap<Invoice, InvoicesViewDto>()
				.ForMember(
					dest => dest.CurrencyName,
					opt => opt.MapFrom(src => src.Currency.Name)
				).ForMember(
					dest => dest.CustomerName,
					opt => opt.MapFrom(src => src.Customer.Name)
				).ForMember(
					dest => dest.CompanyName,
					opt => opt.MapFrom(src => src.Company.Name)
				);

		}
	}
}
