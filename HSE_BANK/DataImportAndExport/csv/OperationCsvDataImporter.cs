using System;
using System.Collections.Generic;
using System.Globalization;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.csv;

public class OperationCsvDataImporter : GenericDataImporter<OperationCsvDto, Operation>
{
    protected override List<OperationCsvDto> ParseContent(string content)
    {
        var dtos = new List<OperationCsvDto>();
        var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // Если есть заголовок, пропускаем первую строку
        int start = lines[0].Contains("Type") ? 1 : 0;
        for (int i = start; i < lines.Length; i++)
        {
            var parts = lines[i].Split(',');
            if (parts.Length < 6) continue;
            dtos.Add(new OperationCsvDto
            {
                Type = (OperationType)Enum.Parse(typeof(OperationType), parts[0].Trim()),
                BankAccountId = Guid.Parse(parts[1].Trim()),
                Amount = decimal.Parse(parts[2].Trim(), CultureInfo.InvariantCulture),
                Date = DateTime.Parse(parts[3].Trim(), CultureInfo.InvariantCulture),
                CategoryId = Guid.Parse(parts[4].Trim()),
                Description = parts[5].Trim()
            });
        }

        return dtos;
    }

    protected override Operation ConvertDtoToDomain(OperationCsvDto dto)
    {
        return DomainFactory.CreateOperation(dto.Type, dto.BankAccountId, dto.Amount, dto.Date, dto.CategoryId,
            dto.Description);
    }
}

public class OperationCsvDto
{
    public OperationType Type { get; set; }
    public Guid BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; }
}