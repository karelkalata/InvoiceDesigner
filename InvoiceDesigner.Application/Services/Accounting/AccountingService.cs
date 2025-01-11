using InvoiceDesigner.Application.Interfaces.InterfacesAccounting;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Models.Abstract;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Application.Services.Accounting
{
	public class AccountingService : IAccountingService
	{
		private readonly IAccountingRepository _repoAccounting;
		private readonly IDoubleEntrySetupService _serviceDoubleEntrySetup;

		public AccountingService(IAccountingRepository accountingRepository, IDoubleEntrySetupService doubleEntrySetupService)
		{
			_repoAccounting = accountingRepository;
			_serviceDoubleEntrySetup = doubleEntrySetupService;
		}

		public async Task CreateEntriesAsync(AAccountingDocument document, EAccountingDocument typeDocument, EStatus status)
		{
			await _repoAccounting.DeleteAsync(typeDocument, document.Id);
			if (status == EStatus.Approved)
			{
				var doubleEntrySetup = await GetListDoubleEntrySetup(typeDocument);
				foreach (var item in doubleEntrySetup)
				{
					if (item.EntryMode == EEntryMode.Cumulative)
					{
						var doubleEntry = new DoubleEntry
						{
							AccountingDocument = typeDocument,
							DocumentId = document.Id,
							DateTime = document.DateTime,
							Debit = item.Debit,
							DebitAccount = item.DebitAccount,
							DebitAsset1 = GetAssetId(item.DebitAccount.Asset1, document),
							DebitAsset2 = GetAssetId(item.DebitAccount.Asset2, document),
							DebitAsset3 = GetAssetId(item.DebitAccount.Asset3, document),
							Credit = item.Credit,
							CreditAccount = item.CreditAccount,
							CreditAsset1 = GetAssetId(item.CreditAccount.Asset1, document),
							CreditAsset2 = GetAssetId(item.CreditAccount.Asset2, document),
							CreditAsset3 = GetAssetId(item.CreditAccount.Asset3, document),
							Company = document.Company,
							CompanyId = document.CompanyId,
							Currency = document.Currency,
							CurrencyId = document.CurrencyId,
							Amount = GetAmount(item.AmountType, document)
						};
						await _repoAccounting.CreateAsync(doubleEntry);
					}
					else
					{
						switch (typeDocument)
						{
							case EAccountingDocument.Invoice:

								var invoice = document as Invoice;
								if (invoice != null)
									await CreateInvoiceSplitByItemEntriesAsync(item, invoice);

								break;
							default:
								throw new InvalidOperationException($"CreateEntriesAsync: not found function for {typeDocument}");
						}
					}
				}
			}
		}

		private async Task CreateInvoiceSplitByItemEntriesAsync(DoubleEntrySetup doubleEntrySetup, Invoice invoice)
		{
			foreach (AAccountingItem item in invoice.InvoiceItems)
			{
				var accountingEntry = new DoubleEntry
				{
					AccountingDocument = EAccountingDocument.Invoice,
					DocumentId = invoice.Id,
					DateTime = invoice.DateTime,
					Debit = doubleEntrySetup.Debit,
					DebitAccount = doubleEntrySetup.DebitAccount,
					DebitAsset1 = ResolveDebitAsset(doubleEntrySetup.DebitAccount.Asset1, item, invoice),
					DebitAsset2 = ResolveDebitAsset(doubleEntrySetup.DebitAccount.Asset2, item, invoice),
					DebitAsset3 = ResolveDebitAsset(doubleEntrySetup.DebitAccount.Asset3, item, invoice),
					Credit = doubleEntrySetup.Credit,
					CreditAccount = doubleEntrySetup.CreditAccount,
					CreditAsset1 = ResolveDebitAsset(doubleEntrySetup.CreditAccount.Asset1, item, invoice),
					CreditAsset2 = ResolveDebitAsset(doubleEntrySetup.CreditAccount.Asset2, item, invoice),
					CreditAsset3 = ResolveDebitAsset(doubleEntrySetup.CreditAccount.Asset3, item, invoice),
					Company = invoice.Company,
					CompanyId = invoice.CompanyId,
					Currency = invoice.Currency,
					CurrencyId = invoice.CurrencyId,
					Count = item.Quantity,
					Amount = item.GetAmountWithoutTax()
				};
				await _repoAccounting.CreateAsync(accountingEntry);
			}
		}

		private int ResolveDebitAsset(EAssetType assetType, AAccountingItem item, Invoice invoice)
		{
			return assetType == EAssetType.Product ? item.ItemId : GetAssetId(assetType, invoice);
		}

		private decimal GetAmount(EAmountType amountType, AAccountingDocument document)
		{
			switch (amountType)
			{
				case EAmountType.AmountTax:
					return document.GetAmountTax();
				case EAmountType.AmountWithTax:
					return document.GetAmountWithTax();
				case EAmountType.AmountWithoutTax:
					return document.GetAmountWithoutTax();
				default:
					return 0;
			}
		}

		private int GetAssetId(EAssetType assetsType, AAccountingDocument document)
		{
			switch (assetsType)
			{
				case EAssetType.Bank:
					return document.GetBankId();
				case EAssetType.Customer:
					return document.GetCustomerId();
				default:
					return 0;
			}
		}

		private async Task<List<DoubleEntrySetup>> GetListDoubleEntrySetup(EAccountingDocument typeDocument)
		{
			var query = new QueryPagedDoubleEntrySetup
			{
				Page = 1,
				PageSize = await _serviceDoubleEntrySetup.GetCountByTypeDocumentAsync(typeDocument),
				AccountingDocument = typeDocument
			};
			return await _serviceDoubleEntrySetup.GetEntitiesAsync(query);
		}
	}
}
