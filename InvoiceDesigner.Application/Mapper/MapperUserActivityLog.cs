using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.DtoActivityLog;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperUserActivityLog : Profile
	{
		public MapperUserActivityLog()
		{
			CreateMap<UserActivityLog, UserActivityLogViewDto>()
				.ForMember(
					dest => dest.UserName,
					opt => opt.MapFrom(src => src.User.Name)
				);
		}
	}
}
