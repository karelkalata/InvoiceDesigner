﻿using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperUser : Profile
	{
		public MapperUser()
		{

			CreateMap<User, UserViewDto>();

			CreateMap<User, UserEditDto>();

			CreateMap<User, AdminUserEditDto>();

		}
	}
}
