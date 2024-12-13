using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner;
using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.ServiceFormDesigner
{
	public class FormDesignersService : IFormDesignersService
	{

		private readonly IFormDesignersRepository _repository;
		private readonly IMapper _mapper;
		private readonly IDropItemsService _dropItemsService;
		private readonly IUserActivityLogService _userActivity;

		public FormDesignersService(IFormDesignersRepository repository,
									IMapper mapper,
									IDropItemsService dropItemsService,
									IUserActivityLogService userActivity)
		{
			_repository = repository;
			_mapper = mapper;
			_dropItemsService = dropItemsService;
			_userActivity = userActivity;
		}

		public async Task<IReadOnlyCollection<FormDesignersAutocompleteDto>> GetAllAutocompleteDto()
		{
			var formDesigners = await _repository.GetAllFormDesignersAsync();
			return _mapper.Map<IReadOnlyCollection<FormDesignersAutocompleteDto>>(formDesigners);
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, FormDesignerEditDto formDesignerEditDto)
		{
			ValidateInputAsync(formDesignerEditDto);

			var existsFormDesigner = new FormDesigner();
			var emptySchemes = new List<FormDesignerScheme>();

			for (int i = 0; i < Constants.SetupRows; i++)
			{
				emptySchemes.Add(new FormDesignerScheme
				{
					Row = i,
					Column = 0,
				});
			}
			existsFormDesigner.Schemes = emptySchemes;
			existsFormDesigner.DropItems = _dropItemsService.CreateListDropItems(existsFormDesigner);

			MapFormDesigner(existsFormDesigner, formDesignerEditDto);

			var entityId = await _repository.CreateFormDesignerAsync(existsFormDesigner);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.FormDesigner, EActivitiesType.Create, entityId);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<FormDesigner> GetByIdAsync(int id)
		{
			var formDesigner = await ValidateExistsEntityAsync(id);
			return formDesigner;
		}

		public async Task<FormDesignerEditDto> GetEditDtoByIdAsync(int id)
		{
			var formDesigner = await ValidateExistsEntityAsync(id);
			formDesigner.DropItems = _dropItemsService.CreateListDropItems(formDesigner);

			return _mapper.Map<FormDesignerEditDto>(formDesigner);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, FormDesignerEditDto formDesignerEditDto)
		{
			var existsFormDesigner = await ValidateExistsEntityAsync(formDesignerEditDto.Id);
			ValidateInputAsync(formDesignerEditDto);

			MapFormDesigner(existsFormDesigner, formDesignerEditDto);

			var entityId = await _repository.UpdateFormDesignerAsync(existsFormDesigner);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.FormDesigner, EActivitiesType.Update, entityId);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<ResponseBoolean> DeleteAsync(int userId, int id)
		{
			var existsFormDesigner = await ValidateExistsEntityAsync(id);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.FormDesigner, EActivitiesType.Delete, existsFormDesigner.Id);
			return new ResponseBoolean
			{
				Result = await _repository.DeleteFormDesignerAsync(existsFormDesigner)
			};
		}

		public DropItemEditDto AddEmptyBox()
		{
			var item = _dropItemsService.AddEmptyBox();
			return _mapper.Map<DropItemEditDto>(item);
		}

		private async Task<FormDesigner> ValidateExistsEntityAsync(int id)
		{
			var existsFormDesigner = await _repository.GetFormDesignerByIdAsync(id)
				?? throw new InvalidOperationException("FormDesigner not found");
			return existsFormDesigner;
		}

		private void ValidateInputAsync(FormDesignerEditDto dto)
		{
			if (string.IsNullOrEmpty(dto.Name))
				throw new InvalidOperationException("Value can't be empty.");

		}

		private void MapFormDesigner(FormDesigner existsFormDesigner, FormDesignerEditDto dto)
		{
			existsFormDesigner.Name = dto.Name.Trim();
			existsFormDesigner.DropItems = _dropItemsService.MapDropItems(dto.DropItems);

			var newSchemes = new List<FormDesignerScheme>();

			//maybe the layout of the pdf document has been changed? we only update up-to-date information
			for (int i = 0; i < Constants.SetupRows; i++)
			{
				var scheme = new FormDesignerScheme();
				if (dto.Schemes.Count <= i || dto.Schemes.ElementAt(i) is null)
				{
					scheme.Row = i;
				}
				else
				{
					var dtoScheme = dto.Schemes.ElementAt(i);
					scheme.Row = dtoScheme.Row;
					scheme.Column = dtoScheme.Column;
				}
				newSchemes.Add(scheme);
			}

			existsFormDesigner.Schemes = newSchemes;
		}

	}
}
