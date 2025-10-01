﻿using AutoMapper;
using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperCustomer : Profile
	{
		public MapperCustomer()
		{
			CreateMap<Customer, CustomerAutocompleteDto>();

			CreateMap<Customer, CustomerEditDto>();

			CreateMap<Customer, CustomerViewDto>();

			CreateMap<Customer, CustomerPrintDto>();
		}
	}
}
