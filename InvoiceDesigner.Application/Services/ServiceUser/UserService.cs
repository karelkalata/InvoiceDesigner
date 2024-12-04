using AutoMapper;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.ServiceUser
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repository;
		private readonly IMapper _mapper;
		private readonly ICompanyService _companyService;

		public UserService(IUserRepository repository, IMapper mapper, ICompanyService companyService)
		{
			_repository = repository;
			_mapper = mapper;
			_companyService = companyService;
		}

		public async Task<ResponsePaged<UserViewDto>> GetPagedUsersAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var usersTask = _repository.GetUsersAsync(queryPaged, GetOrdering(queryPaged.SortLabel));
			var totalCountTask = _repository.GetCountUsersAsync(queryPaged.ShowDeleted);

			await Task.WhenAll(usersTask, totalCountTask);

			var usersDtos = _mapper.Map<IReadOnlyCollection<UserViewDto>>(await usersTask);
			return new ResponsePaged<UserViewDto>
			{
				Items = usersDtos,
				TotalCount = await totalCountTask
			};
		}

		public async Task<ResponseRedirect> CreateAdminUserAsync(AdminUserEditDto adminUserEditDto)
		{
			var existUser = new User();
			await MapUser(existUser, adminUserEditDto);

			var entityId = await _repository.CreateUserAsync(existUser);

			return new ResponseRedirect
			{
				RedirectUrl = "/Admin/Users"
			};
		}

		public async Task<UserEditDto> GetUserEditDtoByIdAsync(int id)
		{
			var entity = await ValidateExistsEntityAsync(id);
			return _mapper.Map<UserEditDto>(entity);
		}

		public async Task<AdminUserEditDto> GetAdminUserEditDtoByIdAsync(int id)
		{
			var entity = await ValidateExistsEntityAsync(id);
			return _mapper.Map<AdminUserEditDto>(entity);
		}

		public async Task<ResponseRedirect> UpdateUserAsync(UserEditDto dto)
		{
			var existEntity = await ValidateExistsEntityAsync(dto.Id);

			MapUser(existEntity, dto);

			var entityId = await _repository.UpdateUserAsync(existEntity);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty
			};
		}

		public async Task<ResponseRedirect> UpdateAdminUserAsync(AdminUserEditDto dto)
		{
			var existEntity = await ValidateExistsEntityAsync(dto.Id);

			await MapUser(existEntity, dto);

			var entityId = await _repository.UpdateUserAsync(existEntity);

			return new ResponseRedirect
			{
				RedirectUrl = "/Admin/Users"
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity)
		{
			var existsEntity = await ValidateExistsEntityAsync(queryDeleteEntity.EntityId);

			if (!queryDeleteEntity.MarkAsDeleted)
			{
				return new ResponseBoolean
				{
					Result = await _repository.DeleteUserAsync(existsEntity)
				};
			}
			else
			{
				existsEntity.IsDeleted = true;
				await _repository.UpdateUserAsync(existsEntity);

				return new ResponseBoolean
				{
					Result = true
				};  
			}
		}

		public async Task<ResponseBoolean> CheckLoginName(string loginName)
		{
			var result = new ResponseBoolean();
			if (string.IsNullOrEmpty(loginName))
			{
				result.Result = true;
			}
			else
			{
				var existsUser = await _repository.GetUserByLoginAsync(loginName);
				result.Result = existsUser != null;
			}

			return result;
		}

		private async Task<User> ValidateExistsEntityAsync(int id)
		{
			var user = await _repository.GetUserByIdAsync(id)
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

		private void ValidateInputAsync(AdminUserEditDto dto)
		{
			if (string.IsNullOrEmpty(dto.Login))
				throw new InvalidOperationException($"Login can't be empty");

			if (string.IsNullOrEmpty(dto.Password))
				throw new InvalidOperationException($"Password can't be empty");

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

		private async Task MapUser(User existUser, AdminUserEditDto dto)
		{
			existUser.Name = dto.Name.Trim();
			existUser.Login = dto.Login.Trim();

			if (!string.IsNullOrEmpty(dto.Password))
			{
				var (hash, salt) = UserPaswordHasher.CreateHash(dto.Password);
				existUser.PasswordHash = hash;
				existUser.PasswordSalt = salt;
			}

			existUser.Companies = await MapUserCompany(dto.Companies);
		}

		private async Task<List<Company>> MapUserCompany(ICollection<CompanyAutocompleteDto> companyAutocompleteDtos)
		{
			List<Company> result = new List<Company>();
			foreach (var item in companyAutocompleteDtos)
			{
				var company = await _companyService.GetCompanyByIdAsync(item.Id);
				if (company != null)
					result.Add(company);
			}

			return result;
		}

		private Func<IQueryable<User>, IOrderedQueryable<User>> GetOrdering(string sortLabel)
		{
			var orderingOptions = new Dictionary<string, Func<IQueryable<User>, IOrderedQueryable<User>>>
			{
				{"Id_desc", q => q.OrderByDescending(e => e.Id)},
				{"Value", q => q.OrderBy(e => e.Name)},
				{"Name_desc", q => q.OrderByDescending(e => e.Name)}
			};

			return orderingOptions.GetValueOrDefault(sortLabel, q => q.OrderBy(e => e.Id));
		}
	}
}
