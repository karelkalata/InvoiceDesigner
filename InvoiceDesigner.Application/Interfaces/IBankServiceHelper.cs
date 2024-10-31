﻿using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IBankServiceHelper
	{
		Task<bool> IsCurrencyUsedInBanks(int currencyId);
	}
}