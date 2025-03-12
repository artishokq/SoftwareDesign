using System;
using System.Collections.Generic;
using System.Globalization;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.csv;

public class BankAccountCsvDataImporter : GenericDataImporter<BankAccountCsvDto, BankAccount>
{
    protected override List<BankAccountCsvDto> ParseContent(string content)
    {
        var dtos = new List<BankAccountCsvDto>();
        var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // Пропускаем заголовок, если он есть
        int start = lines[0].Contains("Id") ? 1 : 0;
        for (int i = start; i < lines.Length; i++)
        {
            var parts = lines[i].Split(',');
            if (parts.Length < 3) continue;
            dtos.Add(new BankAccountCsvDto
            {
                Id = Guid.Parse(parts[0].Trim()),
                Name = parts[1].Trim(),
                Balance = decimal.Parse(parts[2].Trim(), CultureInfo.InvariantCulture)
            });
        }

        return dtos;
    }

    protected override BankAccount ConvertDtoToDomain(BankAccountCsvDto dto)
    {
        var account = new BankAccount(dto.Name, dto.Balance);
        typeof(BankAccount).GetProperty("Id").SetValue(account, dto.Id);
        return account;
    }
}

public class BankAccountCsvDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}