using System.Collections.Generic;
using HSE_BANK.DataImportAndExport.json;
using HSE_BANK.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HSE_BANK.DataImportAndExport.yaml;

public class CategoryYamlDataImporter : GenericDataImporter<CategoryDto, Category>
{
    protected override List<CategoryDto> ParseContent(string content)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<List<CategoryDto>>(content);
    }

    protected override Category ConvertDtoToDomain(CategoryDto dto)
    {
        var category = new Category(dto.Type, dto.Name);
        typeof(Category).GetProperty("Id").SetValue(category, dto.Id);
        return category;
    }
}