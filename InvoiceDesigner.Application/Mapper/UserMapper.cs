using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public static class UserMapper
	{
		public static IReadOnlyCollection<UserViewDto> ToViewDto(IReadOnlyCollection<User> users) => users.Select(ToViewDto).ToList();

		public static UserEditDto ToEditDto(User user)
		{
			return new UserEditDto
			{
				Id = user.Id,
				Name = user.Name,
				Locale = user.Locale,
			};
		}

		public static UserViewDto ToViewDto(User user)
		{
			return new UserViewDto
			{
				Id = user.Id,
				Login = user.Login,
				Name = user.Name,
				IsDeleted = user.IsDeleted,
				IsAdmin = user.IsAdmin
			};
		}

		public static AdminUserEditDto ToEditAdminUserDto(User user)
		{
			return new AdminUserEditDto
			{
				Id = user.Id,
				Login = user.Login,
				Name = user.Name,
				IsAdmin = user.IsAdmin,
				Password = string.Empty,
				Locale = user.Locale,
				Companies = user.Companies.Select(CompanyMapper.ToAutocompleteDto).ToList()
			};
		}

	}

}
