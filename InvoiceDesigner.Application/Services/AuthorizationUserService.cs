﻿using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
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

		public AuthorizationUserService(IUserRepository repository, IConfiguration configuration)
		{
			_repository = repository;
			_configuration = configuration;
		}

		public async Task<ResponseJwtToken> LoginAsync(UserLoginDto dto)
		{
			var result = new ResponseJwtToken
			{
				JwtToken = string.Empty
			};

			var existUser = await _repository.GetByLoginAsync(dto.Login);
			if (existUser == null || existUser.IsDeleted)
				return result;

			if (!UserPaswordHasher.VerifyPassword(dto.Password, existUser.PasswordHash, existUser.PasswordSalt))
			{
				return result;
			}

			result.Locale = existUser.Locale;

			var secretKey = _configuration["JWTOption:SecretKey"];
			if (secretKey == null)
				throw new InvalidOperationException("JWTOption:SecretKey is null");

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

	}
}
