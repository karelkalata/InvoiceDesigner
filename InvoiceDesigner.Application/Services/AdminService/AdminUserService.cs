using AutoMapper;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.AdminInterfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
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
		private readonly IUserActivityLogService _userActivity;

		public AdminUserService(IUserRepository repoUser, 
								IMapper mapper, 
								ICompanyService companyService,
								IUserActivityLogService userActivity)
		{
			_repoUser = repoUser;
			_mapper = mapper;
			_companyService = companyService;
			_userActivity = userActivity;
		}

		public async Task<ResponsePaged<UserViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var usersTask = _repoUser.GetUsersAsync(queryPaged, GetOrdering(queryPaged.SortLabel));
			var totalCountTask = _repoUser.GetCountUsersAsync(queryPaged.ShowDeleted);

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

			var entityId = await _repoUser.CreateUserAsync(existUser);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.User, EActivitiesType.Create, entityId);

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

			var entityId = await _repoUser.UpdateUserAsync(existEntity);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.User, EActivitiesType.Update, entityId);

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
				await _userActivity.CreateActivityLog(queryDeleteEntity.UserId, EDocumentsTypes.User, EActivitiesType.Delete, existsEntity.Id);
				return new ResponseBoolean
				{
					Result = await _repoUser.DeleteUserAsync(existsEntity)
				};
			}
			else
			{
				existsEntity.IsDeleted = true;
				await _repoUser.UpdateUserAsync(existsEntity);
				await _userActivity.CreateActivityLog(queryDeleteEntity.UserId, EDocumentsTypes.User, EActivitiesType.MarkedAsDeleted, existsEntity.Id);

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
				var existsUser = await _repoUser.GetUserByLoginAsync(loginName);
				result.Result = existsUser != null;
			}

			return result;
		}

		private async Task<User> ValidateExistsEntityAsync(int id)
		{
			var user = await _repoUser.GetUserByIdAsync(id)
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
