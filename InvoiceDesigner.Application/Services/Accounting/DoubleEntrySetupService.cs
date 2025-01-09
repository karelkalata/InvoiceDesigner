using AutoMapper;
using InvoiceDesigner.Application.Interfaces.InterfacesAccounting;
using InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.Accounting
{
	public class DoubleEntrySetupService : IDoubleEntrySetupService
	{
		private readonly IDoubleEntrySetupRepository _repoDoubleEntrySetup;
		private readonly IChartOfAccountsService _serviceChartOfAccounts;
		private readonly IMapper _mapper;

		public DoubleEntrySetupService(IDoubleEntrySetupRepository doubleEntryRepository, IChartOfAccountsService chartOfAccountsService, IMapper mapper)
		{
			_repoDoubleEntrySetup = doubleEntryRepository;
			_serviceChartOfAccounts = chartOfAccountsService;
			_mapper = mapper;
		}

		public async Task<ResponsePaged<DoubleEntrySetupEditDto>> GetPagedAsync(QueryPagedDoubleEntrySetup queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var entitiesTask = _repoDoubleEntrySetup.GetEntitiesAsync(queryPaged);
			var totalCountTask = _repoDoubleEntrySetup.GetCountAsync();

			await Task.WhenAll(entitiesTask, totalCountTask);

			var entitiesViewDto = _mapper.Map<IReadOnlyCollection<DoubleEntrySetupEditDto>>(await entitiesTask);

			return new ResponsePaged<DoubleEntrySetupEditDto>
			{
				Items = entitiesViewDto,
				TotalCount = await totalCountTask
			};
		}

		public async Task<List<DoubleEntrySetup>> GetEntitiesAsync(QueryPagedDoubleEntrySetup queryPaged)
		{
			var entities = await _repoDoubleEntrySetup.GetEntitiesAsync(queryPaged);
			return entities.ToList();
		}

		public async Task<ResponseRedirect> CreateAsync(DoubleEntrySetupEditDto createDto)
		{
			var existsEntity = new DoubleEntrySetup();

			await MapDtoToEntity(existsEntity, createDto);

			var entityId = await _repoDoubleEntrySetup.CreateAsync(existsEntity);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<ResponseRedirect> UpdateAsync(DoubleEntrySetupEditDto editedDto)
		{
			var existsEntity = await ValidateExistsEntityAsync(editedDto.Id);

			await MapDtoToEntity(existsEntity, editedDto);

			var entityId = await _repoDoubleEntrySetup.UpdateAsync(existsEntity);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty
			};
		}

		public async Task<ResponseBoolean> DeleteAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return new ResponseBoolean
			{
				Result = await _repoDoubleEntrySetup.DeleteAsync(existsEntity)
			};
		}

		public async Task<int> GetCountByTypeDocumentAsync(EAccountingDocument typeDocument)
		{
			return await _repoDoubleEntrySetup.GetCountByTypeDocumentAsync(typeDocument);
		}

		public async Task<DoubleEntrySetup> ValidateExistsEntityAsync(int id)
		{
			return await _repoDoubleEntrySetup.GetByIdAsync(id)
				?? throw new InvalidOperationException($"A record with ID {id} not found in DoubleEntrySetup");
		}

		private async Task MapDtoToEntity(DoubleEntrySetup existsEntity, DoubleEntrySetupEditDto dto)
		{
			existsEntity.AccountingDocument = dto.AccountingDocument;

			var debit = await _serviceChartOfAccounts.ValidateExistsEntityAsync(dto.DebitAccount.Id);
			existsEntity.Debit = debit.Id;
			existsEntity.DebitAccount = debit;

			var credit = await _serviceChartOfAccounts.ValidateExistsEntityAsync(dto.CreditAccount.Id);
			existsEntity.Credit = credit.Id;
			existsEntity.CreditAccount = credit;

			existsEntity.EntryMode = dto.EntryMode;
			existsEntity.AmountType = dto.AmountType;
		}

	}
}
