using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InvoiceDesigner.Application.Services
{
	public class AuthorizationUserService : IAuthorizationUserService
	{
		private readonly IUserRepository _repository;
		private readonly IConfiguration _configuration;
		private readonly IUserActivityLogService _serviceUserActivityLog;

		public AuthorizationUserService(IUserRepository repository, IConfiguration configuration, IUserActivityLogService userActivity)
		{
			_repository = repository;
			_configuration = configuration;
			_serviceUserActivityLog = userActivity;
		}

		public async Task<ResponseJwtToken> LoginAsync(UserLoginDto dto)
		{
			var result = new ResponseJwtToken
			{
				JwtToken = string.Empty
			};

			var existUser = await _repository.GetUserByLoginAsync(dto.Login);
			if (existUser == null || existUser.IsDeleted)
				return result;

			if (!UserPaswordHasher.VerifyPassword(dto.Password, existUser.PasswordHash, existUser.PasswordSalt))
			{
				await _serviceUserActivityLog.UserLogin(existUser, EActivitiesType.LoginFailure);
				return result;
			}

			result.Locale = existUser.Locale;

			var secretKey = _configuration["JWTOption:SecretKey"];
			if (secretKey == null)
				throw new InvalidOperationException("JWTOption:SecretKey is null");

			await _serviceUserActivityLog.UserLogin(existUser, EActivitiesType.LoginSuccess);

			var signingCredentials = new SigningCredentials(
										new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
										SecurityAlgorithms.HmacSha256
									);

			var expiresHoursString = _configuration["JWTOption:ExpiresHours"];
			if (!double.TryParse(expiresHoursString, out double expiresHours))
				expiresHours = 8;

			var token = new JwtSecurityToken(
							claims: [
								new("userId" , existUser.Id.ToString(), ClaimValueTypes.Integer),
								new("isAdmin", existUser.IsAdmin.ToString(), ClaimValueTypes.Boolean),
								new("userName", existUser.Name.ToString(), ClaimValueTypes.String)
							],
							signingCredentials: signingCredentials,
							expires: DateTime.UtcNow.AddHours(expiresHours)
						);

			result.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
			return result;


		}

		public async Task LogoutUser(int userId)
		{
			var existUser = await _repository.GetUserByIdAsync(userId);
			if (existUser != null)
			{
				await _serviceUserActivityLog.UserLogout(existUser);
			}
		}
	}
}
