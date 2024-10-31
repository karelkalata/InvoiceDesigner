using AutoMapper;
using InvoiceDesigner.Application.Helpers;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.FormDesigners;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models.FormDesigner;

namespace InvoiceDesigner.Application.Services
{
	public class FormDesignersService : IFormDesignersService
	{
		private readonly IFormDesignersRepository _repository;
		private readonly IMapper _mapper;

		public FormDesignersService(IFormDesignersRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IReadOnlyCollection<FormDesignersAutocompleteDto>> GetAllFormDesignersAutocompleteDto()
		{
			var formDesigners = await _repository.GetAllFormDesignersAsync();
			return _mapper.Map<IReadOnlyCollection<FormDesignersAutocompleteDto>>(formDesigners);
		}

		public async Task<ResponseRedirect> CreateFormDesignerAsync(FormDesignerEditDto formDesignerEditDto)
		{
			ValidateInputAsync(formDesignerEditDto);

			var existsFormDesigner = new FormDesigner();
			MapInvoice(existsFormDesigner, formDesignerEditDto);

			var entityId = await _repository.CreateFormDesignerAsync(existsFormDesigner);

			return new ResponseRedirect
			{
				RedirectUrl = $"/FormDesigner/{entityId}"
			};
		}

		public async Task<FormDesigner> GetFormDesignerByIdAsync(int id)
		{
			var formDesigner = await ValidateExistsEntityAsync(id);
			formDesigner.DropItems = AddListDropItemsDto(formDesigner);
			return formDesigner;
		}

		public async Task<FormDesignerEditDto> GetFormDesignerEditDtoByIdAsync(int id)
		{
			var formDesigner = await ValidateExistsEntityAsync(id);
			formDesigner.DropItems = AddListDropItemsDto(formDesigner);
			return _mapper.Map<FormDesignerEditDto>(formDesigner);
		}

		public async Task<ResponseRedirect> UpdateFormDesignerAsync(FormDesignerEditDto formDesignerEditDto)
		{
			var existsFormDesigner = await ValidateExistsEntityAsync(formDesignerEditDto.Id);
			ValidateInputAsync(formDesignerEditDto);

			MapInvoice(existsFormDesigner, formDesignerEditDto);

			var entityId = await _repository.UpdateFormDesignerAsync(existsFormDesigner);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty
			};
		}

		public async Task<bool> DeleteFormDesignerAsync(int id)
		{
			var existsFormDesigner = await ValidateExistsEntityAsync(id);
			return await _repository.DeleteFormDesignerAsync(existsFormDesigner);
		}

		public DropItemEditDto AddEmptyBox()
		{
			var item = new DropItem
			{
				UniqueId = Guid.NewGuid().ToString(),
				Name = "input text here",
				Selector = "_Other",
				StartSelector = "_Other",
				CssStyle = DefaultDropItemStyles.GetDefaultStyles()
			};
			return _mapper.Map<DropItemEditDto>(item);
		}

		private async Task<FormDesigner> ValidateExistsEntityAsync(int id)
		{
			var existsFormDesigner = await _repository.GetFormDesignerByIdAsync(id)
				?? throw new InvalidOperationException("FormDesigner not found");
			return existsFormDesigner;
		}

		private ICollection<DropItem> AddListDropItemsDto(FormDesigner formDesigner)
		{
			var result = new List<DropItem>(formDesigner.DropItems);
			var printableTypes = GetTypesImplementingInterface<IPrintable>();

			foreach (var printDto in printableTypes)
			{
				if (Activator.CreateInstance(printDto) is IPrintable instance)
				{
					foreach (var property in instance.GetType().GetProperties())
					{
						var name = $"{{{instance.GetSelectorName()}.{property.Name}}}";
						if (result.All(e => e.UniqueId != name))
						{
							result.Add(CreateDropItem(name, formDesigner.Id, instance.GetSelectorName()));
						}
					}
				}
			}

			if (result.All(e => e.UniqueId != "{Table.Footer}"))
			{
				result.Add(new DropItem
				{
					FormDesignerId = formDesigner.Id,
					UniqueId = "{Table.Footer}",
					Name = "Total Invoice Items",
					Selector = "_Hidden",
					StartSelector = "_Hidden",
					CssStyle = DefaultDropItemStyles.GetDefaultStyles()
				});
			}

			int existsHidden = result.Count(e => e.StartSelector == "_Hidden");
			for (var i = existsHidden; i < formDesigner.Columns; i++)
			{
				result.Add(new DropItem
				{
					FormDesignerId = formDesigner.Id,
					UniqueId = Guid.NewGuid().ToString(),
					Name = "",
					Selector = "_Hidden",
					StartSelector = "_Hidden",
					CssStyle = DefaultDropItemStyles.GetDefaultStyles()
				});
			}

			return result;
		}

		private DropItem CreateDropItem(string uniqueId, int formDesignerId, string selectorName)
		{
			return new DropItem
			{
				FormDesignerId = formDesignerId,
				UniqueId = uniqueId,
				Name = uniqueId,
				Selector = $"_{selectorName}",
				StartSelector = $"_{selectorName}",
				CssStyle = uniqueId == "{Invoice.InvoiceItems}" ? DefaultDropItemStyles.GetDefaultStylesItems() : DefaultDropItemStyles.GetDefaultStyles()
			};
		}

		private void ValidateInputAsync(FormDesignerEditDto formDesignerEditDto)
		{
			if (string.IsNullOrEmpty(formDesignerEditDto.Name))
				throw new InvalidOperationException("Name can't be empty.");
		}

		private void ProcessDropItem(FormDesigner formDesigner, DropItem? existsDropItem, DropItemEditDto itemEditDto)
		{
			if (!itemEditDto.Selector.StartsWith("coor_"))
			{
				if (existsDropItem != null)
				{
					formDesigner.DropItems.Remove(existsDropItem);
					_repository.DeleteDropItemsFromContext(existsDropItem);
				}
				return;
			}

			if (existsDropItem == null)
			{
				existsDropItem = new DropItem
				{
					Name = itemEditDto.Name,
					UniqueId = itemEditDto.UniqueId,
					Selector = itemEditDto.Selector,
					StartSelector = itemEditDto.StartSelector,
					FormDesignerId = formDesigner.Id
				};
				formDesigner.DropItems.Add(existsDropItem);
			}
			else
			{
				existsDropItem.Name = itemEditDto.Name;
				existsDropItem.Selector = itemEditDto.Selector;
			}

			foreach (var itemCssStyle in itemEditDto.CssStyleEditDto)
			{
				var existsCssStyle = existsDropItem.CssStyle.FirstOrDefault(a => a.Name == itemCssStyle.Name)
					?? new DropItemCssStyle();
				existsCssStyle.Name = itemCssStyle.Name;
				existsCssStyle.Value = itemCssStyle.Value;

				if (!existsDropItem.CssStyle.Contains(existsCssStyle))
				{
					existsDropItem.CssStyle.Add(existsCssStyle);
				}
			}
		}

		public static List<Type> GetTypesImplementingInterface<TInterface>()
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(t => typeof(TInterface).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
				.ToList();
		}

		private void MapInvoice(FormDesigner existsFormDesigner, FormDesignerEditDto dto)
		{
			existsFormDesigner.Name = dto.Name;
			existsFormDesigner.Rows = dto.Rows;
			existsFormDesigner.Columns = dto.Columns;

			List<DropItem> dropItems = new List<DropItem>();
			foreach (var item in dto.DropItemsDto)
			{
				var existsDropItem = existsFormDesigner.DropItems.FirstOrDefault(e => e.UniqueId == item.UniqueId);

				if (!item.Selector.StartsWith("coor_"))
				{
					if (existsDropItem != null)
					{
						existsFormDesigner.DropItems.Remove(existsDropItem);
						_repository.DeleteDropItemsFromContext(existsDropItem);
					}
					continue;
				}
				else
				{
					if (existsDropItem == null)
						existsDropItem = new DropItem();
				}

				existsDropItem.UniqueId = item.UniqueId;
				existsDropItem.Name = item.Name;
				existsDropItem.Selector = item.Selector;
				existsDropItem.StartSelector = item.StartSelector;
				existsDropItem.FormDesignerId = existsFormDesigner.Id;
				existsDropItem.FormDesigner = existsFormDesigner;

				List<DropItemCssStyle> cssStyle = new List<DropItemCssStyle>();
				foreach (var cssStyleDto in item.CssStyleEditDto)
				{
					var existsCssStyle = existsDropItem.CssStyle.FirstOrDefault(e => e.Id == cssStyleDto.Id)
											?? new DropItemCssStyle();

					existsCssStyle.Name = cssStyleDto.Name;
					existsCssStyle.Value = cssStyleDto.Value;
					existsCssStyle.DropItem = existsDropItem;
					existsCssStyle.DropItemId = existsDropItem.Id;

					cssStyle.Add(existsCssStyle);
				}
				existsDropItem.CssStyle = cssStyle;
				dropItems.Add(existsDropItem);
			}
			existsFormDesigner.DropItems = dropItems;
		}
	}

}
