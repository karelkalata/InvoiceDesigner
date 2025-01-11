using AutoMapper;
using InvoiceDesigner.Application.Interfaces.InterfacesAccounting;
using InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs;
using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.Accounting
{
	public class ChartOfAccountsService : IChartOfAccountsService
	{
		private readonly IChartOfAccountsRepository _repoChartOfAccounts;
		private readonly IMapper _mapper;

		public ChartOfAccountsService(IChartOfAccountsRepository repositoryChartOfAccounts, IMapper mapper)
		{
			_repoChartOfAccounts = repositoryChartOfAccounts;
			_mapper = mapper;
		}

		public async Task<ResponsePaged<ChartOfAccountsDto>> GetPagedAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var entitiesTask = _repoChartOfAccounts.GetEntitiesAsync(queryPaged, queryPaged.SortLabel);

			var queryGetCount = new QueryGetCount
			{
				ShowArchived = queryPaged.ShowArchived,
				ShowDeleted = queryPaged.ShowDeleted,
			};
			var totalCountTask = _repoChartOfAccounts.GetCountAsync(queryGetCount);

			await Task.WhenAll(entitiesTask, totalCountTask);

			var entitiesViewDto = _mapper.Map<IReadOnlyCollection<ChartOfAccountsDto>>(await entitiesTask);

			return new ResponsePaged<ChartOfAccountsDto>
			{
				Items = entitiesViewDto,
				TotalCount = await totalCountTask
			};
		}

		public async Task<ResponseRedirect> CreateAsync(ChartOfAccountsDto createDto)
		{
			var existsEntity = new ChartOfAccounts();

			await MapDtoToEntity(existsEntity, createDto);

			var entityId = await _repoChartOfAccounts.CreateAsync(existsEntity);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<ResponseRedirect> UpdateAsync(ChartOfAccountsDto editedDto)
		{
			var existsEntity = await ValidateExistsEntityAsync(editedDto.Id);

			await MapDtoToEntity(existsEntity, editedDto);

			await _repoChartOfAccounts.UpdateAsync(existsEntity);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = existsEntity.Id
			};
		}


		public async Task<ResponseBoolean> DeleteAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return new ResponseBoolean
			{
				Result = await _repoChartOfAccounts.DeleteAsync(existsEntity)
			};
		}

		public async Task<IReadOnlyCollection<ChartOfAccountsAutocompleteDto>> FilteringData(string searchText)
		{
			var queryPaged = new QueryPaged
			{
				PageSize = 10,
				Page = 1,
				SearchString = searchText
			};

			var entities = await _repoChartOfAccounts.GetEntitiesAsync(queryPaged, "Name");

			return _mapper.Map<IReadOnlyCollection<ChartOfAccountsAutocompleteDto>>(entities);
		}

		public async Task<ChartOfAccounts> ValidateExistsEntityAsync(int id)
		{
			return await _repoChartOfAccounts.GetByIdAsync(id)
				?? throw new InvalidOperationException($"A record with ID {id} not found in ChartOfAccounts");
		}

		private async Task MapDtoToEntity(ChartOfAccounts existsEntity, ChartOfAccountsDto dto)
		{
			if (existsEntity.Id == 0)
			{
				var entity = await _repoChartOfAccounts.GetByCodeAsync(dto.Code);
				if (entity != null)
					throw new InvalidOperationException($"A record with Code {dto.Code} already exists.");
			}

			if (string.IsNullOrEmpty(dto.Name))
				throw new InvalidOperationException($"Name can't be empty");

			if (dto.Code <= 0)
				throw new InvalidOperationException($"Code can't be empty");

			existsEntity.Code = dto.Code;
			existsEntity.Name = dto.Name.Trim();
			existsEntity.Asset1 = dto.Asset1;
			existsEntity.Asset2 = dto.Asset2;
			existsEntity.Asset3 = dto.Asset3;
		}
	}
}
