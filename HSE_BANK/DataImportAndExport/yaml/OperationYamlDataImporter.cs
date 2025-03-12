using System;
using System.Collections.Generic;
using HSE_BANK.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HSE_BANK.DataImportAndExport.yaml;

public class OperationYamlDataImporter : GenericDataImporter<OperationYamlDto, Operation>
{
    protected override List<OperationYamlDto> ParseContent(string content)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<List<OperationYamlDto>>(content);
    }

    protected override Operation ConvertDtoToDomain(OperationYamlDto dto)
    {
        return DomainFactory.CreateOperation(dto.Type, dto.BankAccountId, dto.Amount, dto.Date, dto.CategoryId,
            dto.Description);
    }
}

public class OperationYamlDto
{
    public OperationType Type { get; set; }
    public Guid BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; }
}