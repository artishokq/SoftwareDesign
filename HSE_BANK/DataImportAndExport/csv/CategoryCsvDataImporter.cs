using System;
using System.Collections.Generic;
using System.Globalization;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.csv;

public class CategoryCsvDataImporter : GenericDataImporter<CategoryCsvDto, Category>
{
    protected override List<CategoryCsvDto> ParseContent(string content)
    {
        var dtos = new List<CategoryCsvDto>();
        var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int start = lines[0].Contains("Id") ? 1 : 0;
        for (int i = start; i < lines.Length; i++)
        {
            var parts = lines[i].Split(',');
            if (parts.Length < 3) continue;
            dtos.Add(new CategoryCsvDto
            {
                Id = Guid.Parse(parts[0].Trim()),
                Name = parts[1].Trim(),
                Type = parts[2].Trim() == "Income" ? CategoryType.Income : CategoryType.Expense
            });
        }

        return dtos;
    }

    protected override Category ConvertDtoToDomain(CategoryCsvDto dto)
    {
        var category = new Category(dto.Type, dto.Name);
        typeof(Category).GetProperty("Id").SetValue(category, dto.Id);
        return category;
    }
}

public class CategoryCsvDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CategoryType Type { get; set; }
}