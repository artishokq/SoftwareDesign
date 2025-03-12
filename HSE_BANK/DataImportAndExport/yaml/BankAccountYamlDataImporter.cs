using System.Collections.Generic;
using HSE_BANK.DataImportAndExport.json;
using HSE_BANK.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HSE_BANK.DataImportAndExport.yaml;

public class BankAccountYamlDataImporter : GenericDataImporter<BankAccountDto, BankAccount>
{
    protected override List<BankAccountDto> ParseContent(string content)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<List<BankAccountDto>>(content);
    }

    protected override BankAccount ConvertDtoToDomain(BankAccountDto dto)
    {
        var account = new BankAccount(dto.Name, dto.Balance);
        typeof(BankAccount).GetProperty("Id").SetValue(account, dto.Id);
        return account;
    }
}