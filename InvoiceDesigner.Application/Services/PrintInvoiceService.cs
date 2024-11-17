using AutoMapper;
using InvoiceDesigner.Application.Helpers;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner;
using InvoiceDesigner.Domain.Shared.DTOs.Invoice;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using InvoiceDesigner.Domain.Shared.Responses;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InvoiceDesigner.Application.Services
{
	public class PrintInvoiceService : IPrintInvoiceService, IDocument
	{
		private InvoicePrintDto _invoicePrintDto = null!;
		private readonly IPrintInvoiceRepository _repository;
		private readonly IInvoiceService _invoiceService;
		private readonly IMapper _mapper;
		private readonly IFormDesignersService _formDesignersService;

		public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
		public DocumentSettings GetSettings() => DocumentSettings.Default;

		public PrintInvoiceService(IPrintInvoiceRepository repository,
									IInvoiceService invoiceService,
									IMapper mapper,
									IFormDesignersService formDesignersService)
		{
			_repository = repository;
			_invoiceService = invoiceService;
			_mapper = mapper;
			_formDesignersService = formDesignersService;
		}

		public void Compose(IDocumentContainer container)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponsePdf> CreatePDF(Guid guid)
		{

			var printInvoice = await _repository.GetPrintInvoicebyGuidAsync(guid)
					?? throw new InvalidOperationException($"PrintInvoice {guid} not found.");

			var invoiceId = printInvoice.InvoiceId;
			var printform = printInvoice.PrintFormId;

			var invoice = await _invoiceService.GetInvoiceByIdAsync(0, true, invoiceId);
			if (invoice == null)
			{
				_invoicePrintDto = (new InvoiceExample()).GetInvoiceExample();
			}
			else
			{
				_invoicePrintDto = _mapper.Map<InvoicePrintDto>(invoice);
			}

			var formDesigner = await _formDesignersService.GetFormDesignerByIdAsync(printform)
					?? throw new InvalidOperationException($"Form Designer: {printform} Not Found");

			var addCurrencySymbol = ConstsCssProperty.Value_None;
			var addCurrencySymbolFooter = ConstsCssProperty.Value_None;
			var fontSizeInt = 12;
			var fontSizeTableItems = 12;
			var addTableFooter = false;

			Document document = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Size(PageSizes.A4.Portrait());
					page.Margin(1, Unit.Centimetre);
					page.PageColor(Colors.White);
					page.DefaultTextStyle(x => x.FontSize(12));

					page.Content().Column(column =>
					{

						foreach (var scheme in formDesigner.Schemes)
						{
							column.Item().Row(row =>
							{
								for (int coorY = 0; coorY <= scheme.Column; coorY++)
								{
									var selector = $"coor_{scheme.Row}_{coorY}";
									var existstDropItem = formDesigner.DropItems.Where(e => e.Selector == selector).FirstOrDefault();

									if (existstDropItem == null)
									{
										row.RelativeItem().Column(col =>
										{
											col.Item().Text(string.Empty);
										});
									}
									else
									{
										var fontSize = existstDropItem.CssStyle.Where(e => e.Name == ConstsCssProperty.FontSize).FirstOrDefault();
										if (fontSize != null)
										{
											var fontSizeString = fontSize.Value.Replace("px", "");
											if (!int.TryParse(fontSizeString, out fontSizeInt))
												fontSizeInt = 12;
										}
										var getText = parsedText(existstDropItem.Value);

										row.RelativeItem().Column(col =>
										{

											if (existstDropItem.UniqueId == "{Invoice.InvoiceItems}")
											{
												addTableFooter = true;

												var _addCurrencySymbol = existstDropItem.CssStyle.Where(e => e.Name == ConstsCssProperty.AddCurrencySymbol).FirstOrDefault();
												if (_addCurrencySymbol != null)
													addCurrencySymbol = _addCurrencySymbol.Value;

												var _addCurrencySymbolFooter = existstDropItem.CssStyle.Where(e => e.Name == ConstsCssProperty.AddCurrencySymbolFooter).FirstOrDefault();
												if (_addCurrencySymbolFooter != null)
													addCurrencySymbolFooter = _addCurrencySymbolFooter.Value;

												fontSizeTableItems = fontSizeInt;

												col.Item().Table(table =>
												{
													table.ColumnsDefinition(columns =>
													{
														columns.RelativeColumn();
														columns.RelativeColumn();
														columns.RelativeColumn();
														columns.RelativeColumn();
													});

													table.Header(header =>
													{
														header.Cell().Element(container => CellStyle(container, fontSizeInt)).Text("Item").Bold();
														header.Cell().Element(container => CellStyle(container, fontSizeInt)).Text("Price").Bold();
														header.Cell().Element(container => CellStyle(container, fontSizeInt)).Text("Quantity").Bold();
														header.Cell().Element(container => CellStyle(container, fontSizeInt)).Text("Total").Bold();

														static IContainer CellStyle(IContainer container, int fontSize)
														{
															return container.DefaultTextStyle(x => x.SemiBold().FontSize(fontSize))
																			.PaddingVertical(5)
																			.BorderBottom(1)
																			.BorderColor(Colors.Grey.Lighten2);
														}
													});

													foreach (var row in _invoicePrintDto.InvoiceItems)
													{
														table.Cell().Element(container => CellStyle(container, fontSizeInt)).Text(row.ProductName.ToString());
														table.Cell().Element(container => CellStyle(container, fontSizeInt)).Text(CheckAddCurrencySymbol(row.Price, addCurrencySymbol, "N2"));
														table.Cell().Element(container => CellStyle(container, fontSizeInt)).Text(row.Quantity.ToString("N0"));
														table.Cell().Element(container => CellStyle(container, fontSizeInt)).Text(CheckAddCurrencySymbol(row.Total, addCurrencySymbol, "N2"));

														static IContainer CellStyle(IContainer container, int fontSize)
														{
															return container.DefaultTextStyle(x => x.FontSize(fontSize))
																			.BorderBottom(1)
																			.BorderColor(Colors.Grey.Lighten3)
																			.PaddingVertical(2);
														}
													}
												});

											}
											else
											{
												col.Item().Text(text =>
												{
													text.Span(getText).FontSize(fontSizeInt);

													var fontStyle = existstDropItem.CssStyle.Any(e => e.Value == ConstsCssProperty.Value_Bold) ? "1" : "0";
													fontStyle += existstDropItem.CssStyle.Any(e => e.Value == ConstsCssProperty.Value_Italic) ? "1" : "0";

													switch (fontStyle)
													{
														case "10":
															text.DefaultTextStyle(TextStyle.Default.Bold());
															break;
														case "01":
															text.DefaultTextStyle(TextStyle.Default.Italic());
															break;
														case "11":
															text.DefaultTextStyle(TextStyle.Default.Italic().Bold());
															break;
													}

													switch (existstDropItem.CssStyle.Where(e => e.Name == ConstsCssProperty.TextAlign).Select(e => e.Value).First())
													{
														case ConstsCssProperty.Value_Center:
															text.AlignCenter();
															break;
														case ConstsCssProperty.Value_Right:
															text.AlignRight();
															break;
														default:
															text.AlignLeft();
															break;
													}

												});
											}
										});
									};
								}

							});

							if (addTableFooter)
							{
								addTableFooter = false;
								column.Item().Row(row =>
								{
									for (int footerColumn = 0; footerColumn < 3; footerColumn++)
									{
										if (footerColumn < 2)
										{
											row.RelativeItem().Column(col =>
											{
												col.Item().Text(string.Empty);
											});
										}
										else
										{
											row.RelativeItem().Column(col =>
											{
												col.Item().Table(tableFooter =>
												{
													tableFooter.ColumnsDefinition(columns =>
													{
														columns.RelativeColumn();
														columns.RelativeColumn();

													});

													decimal subTotal = _invoicePrintDto.InvoiceItems.Sum(item => item.Total);
													decimal subVat = 0;

													if (_invoicePrintDto.EnabledVat)
													{

														tableFooter.Cell().Element(container => CellStyle(container, fontSizeTableItems)).Text("Sub Total: ");
														tableFooter.Cell().Element(container => CellStyle(container, fontSizeTableItems)).Text(CheckAddCurrencySymbol(subTotal, addCurrencySymbolFooter, "N2"));

														subVat = subTotal * _invoicePrintDto.Vat / 100;

														tableFooter.Cell().Element(container => CellStyle(container, fontSizeTableItems)).Text($"VAT({_invoicePrintDto.Vat}%):");
														tableFooter.Cell().Element(container => CellStyle(container, fontSizeTableItems)).Text(CheckAddCurrencySymbol(subVat, addCurrencySymbolFooter, "N2"));
													}

													decimal total = subTotal + subVat;
													tableFooter.Cell().Element(container => CellStyle(container, fontSizeTableItems)).Text("Total:");
													tableFooter.Cell().Element(container => CellStyle(container, fontSizeTableItems)).Text(CheckAddCurrencySymbol(total, addCurrencySymbolFooter, "N2"));


													static IContainer CellStyle(IContainer container, int fontSize)
													{
														return container.DefaultTextStyle(x => x.FontSize(fontSize))
																		.BorderBottom(1)
																		.BorderColor(Colors.Grey.Lighten2)
																		.PaddingVertical(2);
													};
												});
											});
										}
									}
								});

							}
						}
					});
				});
			});

			byte[] pdfBytes = document.GeneratePdf();

			await _repository.DeletePrintInvoicebyGuidAsync(printInvoice);

			return new ResponsePdf()
			{
				ByteArray = pdfBytes,
				FileName = $"Invoice_{_invoicePrintDto.InvoiceNumber}.pdf",
				MimeType = "application/pdf"
			};

		}

		private string CheckAddCurrencySymbol(decimal value, string coorSymbol, string resFormat)
		{
			if (coorSymbol == ConstsCssProperty.Value_Left)
			{
				return $"{_invoicePrintDto.Currency.CurrencySymbol} {value.ToString(resFormat)}";
			}
			else if (coorSymbol == ConstsCssProperty.Value_Right)
			{
				return $"{value.ToString(resFormat)} {_invoicePrintDto.Currency.CurrencySymbol}";
			}
			else
			{
				return value.ToString(resFormat);
			}
		}

		private string parsedText(string str)
		{
			string pattern = @"\{([^\{\}]+)\}";
			MatchCollection matches = Regex.Matches(str, pattern);

			foreach (Match match in matches)
			{
				string foundText = match.Groups[1].Value;
				string[] parts = foundText.Split('.');
				string newStr = string.Empty;

				if (parts.Length == 2)
				{
					string className = parts[0];
					string propertiesName = parts[1];
					switch (className)
					{
						case "Company":
							newStr = GetPropertyValue(_invoicePrintDto.Company, propertiesName);
							break;
						case "Customer":
							newStr = GetPropertyValue(_invoicePrintDto.Customer, propertiesName);
							break;
						case "Currency":
							newStr = GetPropertyValue(_invoicePrintDto.Currency, propertiesName);
							break;
						case "Bank":
							newStr = GetPropertyValue(_invoicePrintDto.Bank, propertiesName);
							break;
						case "Invoice":
							if (propertiesName != "InvoiceItems")
								newStr = GetPropertyValue(_invoicePrintDto, propertiesName);
							break;
						default:
							return foundText;
					}
				}
				str = str.Replace($"{{{foundText}}}", newStr);
			}

			return str;
		}


		public static string GetPropertyValue(object obj, string propertyName)
		{
			PropertyInfo? property = obj.GetType().GetProperty(propertyName);

			if (property != null)
			{
				var value = property.GetValue(obj);

				if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
				{
					DateTime? dateValue = value as DateTime?;
					return dateValue?.ToString("dd.MM.yyyy") ?? $"Error: {propertyName} is null";
				}

				return value?.ToString() ?? $"Error: {propertyName} is null";
			}

			return "not found";
		}

		public async Task<ResponsePdfGuid> GenerateDownloadLink(int invoiceId, int printform)
		{
			var entity = new PrintInvoice
			{
				InvoiceId = invoiceId,
				PrintFormId = printform
			};

			return new ResponsePdfGuid
			{
				Guid = await _repository.GenerateDownloadLinkAsync(entity)
			};
		}

		public Task<ResponsePdf> DownloadPDF(Guid guid)
		{
			throw new NotImplementedException();
		}
	}

}
