using Newtonsoft.Json;
using System.Collections.Generic;
using HSE_BANK.Domain;
using System;

namespace HSE_BANK.DataImportAndExport.json;

public class CategoryJsonDataImporter : GenericDataImporter<CategoryDto, Category>
{
    protected override List<CategoryDto> ParseContent(string content)
    {
        return JsonConvert.DeserializeObject<List<CategoryDto>>(content);
    }

    protected override Category ConvertDtoToDomain(CategoryDto dto)
    {
        var category = new Category(dto.Type, dto.Name);
        typeof(Category).GetProperty("Id").SetValue(category, dto.Id);
        return category;
    }
}

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CategoryType Type { get; set; }
}