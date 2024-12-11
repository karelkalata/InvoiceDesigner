using AutoMapper;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.ServiceUser
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repoUser;
		private readonly IMapper _mapper;

		public UserService(IUserRepository repoUser, IMapper mapper)
		{
			_repoUser = repoUser;
			_mapper = mapper;
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
				throw new InvalidOperationException($"Value can't be empty");

			if (string.IsNullOrEmpty(dto.Password))
				throw new InvalidOperationException($"Password can't be empty");
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
