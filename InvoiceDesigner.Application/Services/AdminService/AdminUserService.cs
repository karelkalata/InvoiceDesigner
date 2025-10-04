using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Interfaces.AdminInterfaces;
using InvoiceDesigner.Application.Mapper;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Application.Services.Abstract;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Localization;

namespace InvoiceDesigner.Application.Services.AdminService
{
	public class AdminUserService : ABaseService<User>, IAdminUserService
	{
		private static string _controllerBaseUrl = "/Admin/Users";

		private readonly IUserRepository _repository;
		private readonly ICompanyRepository _repositoryCompany;

		public AdminUserService(IUserRepository repository, ICompanyRepository repositoryCompany) : base(repository)
		{
			_repository = repository;
			_repositoryCompany = repositoryCompany;
		}

		public async Task<ResponsePaged<UserViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand)
		{
			var (entities, total) = await GetEntitiesAndCountAsync(pagedCommand);

			return new ResponsePaged<UserViewDto>
			{
				Items = UserMapper.ToViewDto(entities),
				TotalCount = total
			};
		}

		public async Task<ResponseRedirect> CreateUserAsync(int userId, AdminUserEditDto dto)
		{
			var existUser = new User();
			await MapUser(existUser, dto);

			await _repository.CreateAsync(existUser);

			return new ResponseRedirect
			{
				RedirectUrl = _controllerBaseUrl
			};
		}

		public async Task<AdminUserEditDto> GetEditDtoByIdAsync(int id)
		{
			var entity = await ValidateExistsEntityAsync(id);
			return UserMapper.ToEditAdminUserDto(entity);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, AdminUserEditDto dto)
		{
			ValidateInputAsync(dto);
			var existEntity = await ValidateExistsEntityAsync(dto.Id);

			await MapUser(existEntity, dto);

			await _repository.UpdateAsync(existEntity);

			return new ResponseRedirect
			{
				RedirectUrl = _controllerBaseUrl
			};
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
				var existsUser = await _repository.GetByLoginAsync(loginName);
				result.Result = existsUser != null;
			}

			return result;
		}

		private async Task<User> ValidateExistsEntityAsync(int id)
		{
			var user = await _repository.GetByIdAsync(new GetByIdFilter { Id = id })
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
				var company = await _repositoryCompany.GetByIdAsync(new GetByIdFilter { Id = item.Id });
				if (company != null)
					result.Add(company);
			}

			return result;
		}
	}
}
