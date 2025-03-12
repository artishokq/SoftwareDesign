using System.Collections.Generic;
using System.IO;

namespace HSE_BANK.DataImportAndExport;

public abstract class GenericDataImporter<TDto, TDomain>
{
    public List<TDomain> ImportData(string filePath)
    {
        var content = File.ReadAllText(filePath);
        var dtos = ParseContent(content);
        var result = new List<TDomain>();
        foreach (var dto in dtos)
        {
            result.Add(ConvertDtoToDomain(dto));
        }

        return result;
    }

    protected abstract List<TDto> ParseContent(string content);
    protected abstract TDomain ConvertDtoToDomain(TDto dto);
}