using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.Interfaces.Abstract;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Application.Services.Abstract
{
	public class ABaseService<T> : IABaseService<T> where T : class, IABaseEntity
	{
		private readonly IABaseRepository<T> _repository;

		public ABaseService(IABaseRepository<T> repository)
		{
			_repository = repository;
		}

		public virtual async Task<(IReadOnlyCollection<T> Entities, int TotalCount)> GetEntitiesAndCountAsync(PagedCommand pagedCommand)
		{

			var pagedFilter = new PagedFilter
			{
				PageSize = pagedCommand.PageSize,
				Page = pagedCommand.Page,
				ShowDeleted = pagedCommand.ShowDeleted,
				ShowArchived = pagedCommand.ShowArchived,
				SearchString = pagedCommand.SearchString,
				SortLabel = pagedCommand.SortLabel,
			};

			var entitiesTask = _repository.GetEntitiesAsync(pagedFilter);
			var totalCountTask = _repository.GetCountAsync(new GetCountFilter
			{
				ShowDeleted = pagedCommand.ShowDeleted,
				ShowArchived = pagedCommand.ShowArchived
			});

			await Task.WhenAll(entitiesTask, totalCountTask);

			return (await entitiesTask, await totalCountTask);
		}


		public virtual async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand)
		{

			var existsEntity = await _repository.GetByIdAsync(new GetByIdFilter { Id = deleteEntityCommand.EntityId });
			var result = false;
			if (existsEntity != null)
			{
				if (!deleteEntityCommand.MarkAsDeleted)
				{
					result = await _repository.DeleteAsync(existsEntity);
				}
				else
				{
					existsEntity.IsDeleted = true;
					result = await _repository.UpdateAsync(existsEntity);
				}
			}
			return new ResponseBoolean { Result = result };
		}
	}
}
