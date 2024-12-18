﻿using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IBankRepository
	{

		Task<IReadOnlyCollection<Bank>> GetAllAsync();

		Task<Bank?> GetByIdAsync(int id);

		Task<bool> IsCurrencyUsedInBanksAsync(int currencyId);

	}
}
