using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Application.Mapper;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Localization;

namespace InvoiceDesigner.Application.Services.ServiceUser
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repoUser;

		public UserService(IUserRepository repoUser)
		{
			_repoUser = repoUser;
		}

		public async Task<UserEditDto> GetEditDtoByIdAsync(int id)
		{
			var entity = await ValidateExistsEntityAsync(id);
			return UserMapper.ToEditDto(entity);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, UserEditDto dto)
		{
			dto.Id = userId; // do not believe the data in the dto sent by the user
			ValidateInputAsync(dto);

			var existEntity = await ValidateExistsEntityAsync(dto.Id);
			MapToUser(existEntity, dto);

			await _repoUser.UpdateAsync(existEntity);

			return new ResponseRedirect
			{
				RedirectUrl = "/Logout"
			};
		}

		private async Task<User> ValidateExistsEntityAsync(int id)
		{
			var user = await _repoUser.GetByIdAsync(new GetByIdFilter { Id = id })
						?? throw new InvalidOperationException("User not found");
			return user;
		}

		private void ValidateInputAsync(UserEditDto dto)
		{
			if (string.IsNullOrEmpty(dto.Name))
				throw new InvalidOperationException($"Name can't be empty");
		}

		private void MapToUser(User existUser, UserEditDto dto)
		{
			existUser.Name = dto.Name.Trim();
			var existsLocalization = Locale.SupportedCultures.FirstOrDefault(c => c.Name.Equals(dto.Locale.Trim(), StringComparison.OrdinalIgnoreCase));
			if (existsLocalization != null)
				existUser.Locale = existsLocalization.Name;
			else
				existUser.Locale = Locale.SupportedCultures[0].ToString();


			if (!string.IsNullOrEmpty(dto.Password))
			{
				var (hash, salt) = UserPaswordHasher.CreateHash(dto.Password);
				existUser.PasswordHash = hash;
				existUser.PasswordSalt = salt;
			}

		}
	}
}
