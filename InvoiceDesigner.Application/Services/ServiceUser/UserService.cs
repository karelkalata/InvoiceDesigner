using AutoMapper;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.ServiceUser
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repoUser;
		private readonly IMapper _mapper;
		private readonly IUserActivityLogService _userActivity;

		public UserService(IUserRepository repoUser, IMapper mapper, IUserActivityLogService userActivity)
		{
			_repoUser = repoUser;
			_mapper = mapper;
			_userActivity = userActivity;
		}

		public async Task<UserEditDto> GetEditDtoByIdAsync(int id)
		{
			var entity = await ValidateExistsEntityAsync(id);
			return _mapper.Map<UserEditDto>(entity);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, UserEditDto dto)
		{
			dto.Id = userId; // do not believe the data in the dto sent by the user
			ValidateInputAsync(dto);

			var existEntity = await ValidateExistsEntityAsync(dto.Id);
			MapUser(existEntity, dto);

			var entityId = await _repoUser.UpdateUserAsync(existEntity);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.User, EActivitiesType.Update, entityId);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty
			};
		}

		private async Task<User> ValidateExistsEntityAsync(int id)
		{
			var user = await _repoUser.GetUserByIdAsync(id)
						?? throw new InvalidOperationException("User not found");
			return user;
		}

		private void ValidateInputAsync(UserEditDto dto)
		{
			if (string.IsNullOrEmpty(dto.Name))
				throw new InvalidOperationException($"Name can't be empty");
		}

		private void MapUser(User existUser, UserEditDto dto)
		{
			existUser.Name = dto.Name.Trim();

			if (!string.IsNullOrEmpty(dto.Password))
			{
				var (hash, salt) = UserPaswordHasher.CreateHash(dto.Password);
				existUser.PasswordHash = hash;
				existUser.PasswordSalt = salt;
			}

		}
	}
}
