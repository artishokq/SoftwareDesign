using Newtonsoft.Json;
using System.Collections.Generic;
using HSE_BANK.Domain;
using System;

namespace HSE_BANK.DataImportAndExport.json;

public class BankAccountJsonDataImporter : GenericDataImporter<BankAccountDto, BankAccount>
{
    protected override List<BankAccountDto> ParseContent(string content)
    {
        return JsonConvert.DeserializeObject<List<BankAccountDto>>(content);
    }

    protected override BankAccount ConvertDtoToDomain(BankAccountDto dto)
    {
        var account = new BankAccount(dto.Name, dto.Balance);
        // Устанавливаем импортированный ID
        typeof(BankAccount).GetProperty("Id").SetValue(account, dto.Id);
        return account;
    }
}

public class BankAccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}