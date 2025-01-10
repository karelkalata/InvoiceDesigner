using AutoMapper;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Interfaces.AdminInterfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;
using InvoiceDesigner.Localization;

namespace InvoiceDesigner.Application.Services.AdminService
{
	public class AdminUserService : IAdminUserInterface
	{
		private static string _controllerBaseUrl = "/Admin/Users";
		private readonly IUserRepository _repoUser;
		private readonly IMapper _mapper;
		private readonly ICompanyService _companyService;

		public AdminUserService(IUserRepository repoUser,
								IMapper mapper,
								ICompanyService companyService)
		{
			_repoUser = repoUser;
			_mapper = mapper;
			_companyService = companyService;
		}

		public async Task<ResponsePaged<UserViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var usersTask = _repoUser.GetEntitiesAsync(queryPaged, queryPaged.SortLabel);

			var queryGetCount = new QueryGetCount
			{
				ShowArchived = queryPaged.ShowArchived,
				ShowDeleted = queryPaged.ShowDeleted,
			};
			var totalCountTask = _repoUser.GetCountAsync(queryGetCount);

			await Task.WhenAll(usersTask, totalCountTask);

			var usersDtos = _mapper.Map<IReadOnlyCollection<UserViewDto>>(await usersTask);
			return new ResponsePaged<UserViewDto>
			{
				Items = usersDtos,
				TotalCount = await totalCountTask
			};
		}

		public async Task<ResponseRedirect> CreateUserAsync(int userId, AdminUserEditDto dto)
		{
			var existUser = new User();
			await MapUser(existUser, dto);

			var entityId = await _repoUser.CreateAsync(existUser);

			return new ResponseRedirect
			{
				RedirectUrl = _controllerBaseUrl
			};
		}

		public async Task<AdminUserEditDto> GetEditDtoByIdAsync(int id)
		{
			var entity = await ValidateExistsEntityAsync(id);
			return _mapper.Map<AdminUserEditDto>(entity);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, AdminUserEditDto dto)
		{
			ValidateInputAsync(dto);
			var existEntity = await ValidateExistsEntityAsync(dto.Id);

			await MapUser(existEntity, dto);

			 await _repoUser.UpdateAsync(existEntity);

			return new ResponseRedirect
			{
				RedirectUrl = _controllerBaseUrl
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity)
		{
			var existsEntity = await ValidateExistsEntityAsync(queryDeleteEntity.EntityId);

			if (!queryDeleteEntity.MarkAsDeleted)
			{
				return new ResponseBoolean
				{
					Result = await _repoUser.DeleteAsync(existsEntity)
				};
			}
			else
			{
				existsEntity.IsDeleted = true;
				await _repoUser.UpdateAsync(existsEntity);

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
				var existsUser = await _repoUser.GetByLoginAsync(loginName);
				result.Result = existsUser != null;
			}

			return result;
		}

		private async Task<User> ValidateExistsEntityAsync(int id)
		{
			var user = await _repoUser.GetByIdAsync(id)
						?? throw new InvalidOperationException("User not found");
			return user;
		}

		private void ValidateInputAsync(AdminUserEditDto dto)
		{
			if (string.IsNullOrEmpty(dto.Name))
				throw new InvalidOperationException($"Name can't be empty");

			if (string.IsNullOrEmpty(dto.Login))
				throw new InvalidOperationException($"Login can't be empty");
		}

		private async Task MapUser(User existUser, AdminUserEditDto dto)
		{
			existUser.Name = dto.Name.Trim();
			existUser.Login = dto.Login.Trim();

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

			existUser.Companies = await MapUserCompany(dto.Companies);
		}

		private async Task<List<Company>> MapUserCompany(ICollection<CompanyAutocompleteDto> companyAutocompleteDtos)
		{
			List<Company> result = new List<Company>();
			foreach (var item in companyAutocompleteDtos)
			{
				var company = await _companyService.GetByIdAsync(item.Id);
				if (company != null)
					result.Add(company);
			}

			return result;
		}
	}
}
