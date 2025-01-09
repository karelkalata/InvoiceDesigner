using AutoMapper;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Interfaces.Documents;
using InvoiceDesigner.Application.Interfaces.InterfacesAccounting;
using InvoiceDesigner.Domain.Shared.DTOs.BankReceiptDTOs;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.Documents
{
	public class BankReceiptService : IBankReceiptService
	{
		private readonly IBankReceiptRepository _repoBankReceipt;
		private readonly IMapper _mapper;
		private readonly ICompanyService _serviceCompany;
		private readonly IInvoiceService _serviceIinvoice;
		private readonly IAccountingService _serviceAccounting;

		public BankReceiptService(IBankReceiptRepository bankReceiptRepository,
									IMapper mapper,
									ICompanyService companyService,
									IInvoiceService invoiceService,
									IAccountingService accountingService)
		{
			_repoBankReceipt = bankReceiptRepository;
			_mapper = mapper;
			_serviceCompany = companyService;
			_serviceIinvoice = invoiceService;
			_serviceAccounting = accountingService;
		}

		public async Task<ResponsePaged<BankReceiptViewDto>> GetPagedAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(queryPaged.UserId, queryPaged.IsAdmin);

			var entities = _repoBankReceipt.GetAsync(queryPaged, GetOrdering(queryPaged.SortLabel), userAuthorizedCompanies);
			var totalCount = _repoBankReceipt.GetCountAsync(queryPaged, userAuthorizedCompanies);

			await Task.WhenAll(entities, totalCount);

			return new ResponsePaged<BankReceiptViewDto>
			{
				Items = _mapper.Map<IReadOnlyCollection<BankReceiptViewDto>>(await entities),
				TotalCount = await totalCount
			};
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, bool isAdmin, BankReceiptCreateDto editedDto)
		{
			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(userId, isAdmin);
			var entity = await CreateBankReceiptFromInvoice(userId, isAdmin, editedDto.InvoiceId);

			entity.Number = await _repoBankReceipt.GetNextNumberForCompanyAsync(entity.CompanyId);

			MapDtoToEntity(entity, editedDto);
			await _serviceAccounting.CreateEntriesAsync(entity, EAccountingDocument.BankReceipt, entity.Status);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = await _repoBankReceipt.CreateAsync(entity)
			};
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, bool isAdmin, BankReceiptCreateDto editedDto)
		{
			var entity = await ValidateExistsEntityAsync(userId, isAdmin, editedDto.Id);
			MapDtoToEntity(entity, editedDto);
			var entityId = await _repoBankReceipt.UpdateAsync(entity);
			await _serviceAccounting.CreateEntriesAsync(entity, EAccountingDocument.BankReceipt, entity.Status);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<BankReceiptViewDto> GetDtoByIdAsync(QueryGetEntity queryGetEntity)
		{

			if (queryGetEntity.EntityId > 0)
			{
				var entity = await ValidateExistsEntityAsync(queryGetEntity.UserId, queryGetEntity.IsAdmin, queryGetEntity.EntityId);
				return _mapper.Map<BankReceiptViewDto>(entity);
			}
			else if (queryGetEntity.ChildEntityId > 0) // queryGetEntity.ChildEntityId = InvoiceId
			{
				var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(queryGetEntity.UserId, queryGetEntity.IsAdmin);
				var entity = await _repoBankReceipt.GetByInvoiceIdAsync(queryGetEntity.ChildEntityId, userAuthorizedCompanies);

				if (entity == null)
				{
					entity = await CreateBankReceiptFromInvoice(queryGetEntity.UserId, queryGetEntity.IsAdmin, queryGetEntity.ChildEntityId);
				}
				return _mapper.Map<BankReceiptViewDto>(entity);
			}
			// WTF???
			throw new InvalidOperationException($"Bad Request QueryGetEntity");
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity)
		{
			var entity = await ValidateExistsEntityAsync(queryDeleteEntity.UserId, queryDeleteEntity.IsAdmin, queryDeleteEntity.EntityId);

			if (!queryDeleteEntity.MarkAsDeleted)
			{
				return new ResponseBoolean
				{
					Result = await _repoBankReceipt.DeleteAsync(entity)
				};
			}
			else
			{
				entity.IsDeleted = true;
				await _repoBankReceipt.UpdateAsync(entity);
				await _serviceAccounting.CreateEntriesAsync(entity, EAccountingDocument.BankReceipt, EStatus.Canceled);

				return new ResponseBoolean
				{
					Result = true
				};
			}
		}

		public async Task<ResponseBoolean> OnChangeProperty(QueryOnChangeProperty query)
		{
			var entity = await ValidateExistsEntityAsync(query.UserId, query.IsAdmin, query.EntityId);
			entity.Status = query.Status;
			entity.IsArchived = query.IsArchived;
			entity.IsDeleted = query.IsDeleted;

			await _repoBankReceipt.UpdateAsync(entity);
			await _serviceAccounting.CreateEntriesAsync(entity, EAccountingDocument.BankReceipt, entity.Status);
			return new ResponseBoolean
			{
				Result = true
			};
		}

		private async Task<BankReceipt> CreateBankReceiptFromInvoice(int userId, bool isAdmin, int invoiceId)
		{
			var existsInvoice = await _serviceIinvoice.GetByIdAsync(userId, isAdmin, invoiceId);

			if (existsInvoice == null)
				throw new InvalidOperationException($"Invoice with ID {invoiceId} was not found or access is denied.");

			return new BankReceipt
			{
				InvoiceId = existsInvoice.Id,
				Invoice = existsInvoice,
				DateTime = DateTime.Now,
				CompanyId = existsInvoice.CompanyId,
				Company = existsInvoice.Company,
				BankId = existsInvoice.BankId,
				Bank = existsInvoice.Bank,
				CurrencyId = existsInvoice.CurrencyId,
				Currency = existsInvoice.Currency,
				CustomerId = existsInvoice.CustomerId,
				Customer = existsInvoice.Customer,
				Amount = existsInvoice.Amount,
			};
		}

		private void MapDtoToEntity(BankReceipt entity, BankReceiptCreateDto editedDto)
		{
			entity.DateTime = editedDto.DateTime ?? DateTime.UtcNow;
			entity.IsArchived = editedDto.IsArchived;
			entity.Status = editedDto.Status;
		}

		private async Task<BankReceipt> ValidateExistsEntityAsync(int userId, bool isAdmin, int entityId)
		{
			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(userId, isAdmin);

			var existsEntity = await _repoBankReceipt.GetByIdAsync(entityId, userAuthorizedCompanies)
							?? throw new InvalidOperationException($"Entity with ID {entityId} was not found or access is denied.");

			return existsEntity;
		}

		private static Func<IQueryable<BankReceipt>, IOrderedQueryable<BankReceipt>> GetOrdering(string sortLabel)
		{
			var orderingOptions = new Dictionary<string, Func<IQueryable<BankReceipt>, IOrderedQueryable<BankReceipt>>>
			{
				{"Id_desc", q => q.OrderByDescending(e => e.Id)},
				{"DateTime", q => q.OrderBy(e => e.DateTime)},
				{"DateTime_desc", q => q.OrderByDescending(e => e.DateTime)}
			};

			return orderingOptions.GetValueOrDefault(sortLabel, q => q.OrderBy(e => e.Id));
		}

	}
}
