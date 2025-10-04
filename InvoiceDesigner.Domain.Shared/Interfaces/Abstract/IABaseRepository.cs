using InvoiceDesigner.Domain.Shared.Filters;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Abstract
{
	public interface IABaseRepository<T> where T : class
	{
		Task<IReadOnlyCollection<T>> GetEntitiesAsync(PagedFilter pagedFilter);
		Task<int> GetCountAsync(GetCountFilter recordGetCount);
		Task CreateAsync(T entity);
		Task<bool> DeleteAsync(T entity);
		Task<bool> UpdateAsync(T entity);
		Task<T?> GetByIdAsync(GetByIdFilter getByIdFilter);



		Func<IQueryable<T>, IOrderedQueryable<T>> GetOrdering(string sortLabel);


	}
}
