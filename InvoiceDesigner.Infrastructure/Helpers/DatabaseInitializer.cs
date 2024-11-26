using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using InvoiceDesigner.Infrastructure.Data;
using System.Text.Json;

namespace InvoiceDesigner.Infrastructure.Helpers
{
	public static class DatabaseInitializer
	{
		public static async Task LoadFormDesignersEntitiesAsync(DataContext context, string filePath)
		{
			if (!context.FormDesigners.Any())
			{
				var jsonData = await File.ReadAllTextAsync(filePath);
				var entities = JsonSerializer.Deserialize<List<FormDesigner>>(jsonData);
				if (entities != null)
				{
					context.FormDesigners.AddRange(entities);
					await context.SaveChangesAsync();
				}
			}

		}
	}
}
