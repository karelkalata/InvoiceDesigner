﻿using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Records;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Abstract
{
	public interface IABaseRepository<T> where T : class
	{
		Task<IReadOnlyCollection<T>> GetEntitiesAsync(QueryPaged queryPaged, string sortLabel);
		Task<int> CreateAsync(T entity);
		Task<T?> GetByIdAsync(int id);
		Task<bool> DeleteAsync(T entity);
		Task UpdateAsync(T entity);
		Task<int> GetCountAsync(GetCountFilter recordGetCount);
		Func<IQueryable<T>, IOrderedQueryable<T>> GetOrdering(string sortLabel);
	}
}
