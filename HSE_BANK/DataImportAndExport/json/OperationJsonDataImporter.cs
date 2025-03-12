using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.json;

public class OperationJsonDataImporter : GenericDataImporter<OperationDto, Operation>
{
    protected override List<OperationDto> ParseContent(string content)
    {
        return JsonConvert.DeserializeObject<List<OperationDto>>(content);
    }

    protected override Operation ConvertDtoToDomain(OperationDto dto)
    {
        return DomainFactory.CreateOperation(dto.Type, dto.BankAccountId, dto.Amount, dto.Date, dto.CategoryId,
            dto.Description);
    }
}

public class OperationDto
{
    public OperationType Type { get; set; }
    public Guid BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; }
}