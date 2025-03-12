using Newtonsoft.Json;
using System.Collections.Generic;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.json;

public class OperationJsonExportVisitor : GenericDataExporter<Operation>
{
    public override string ExportData(IEnumerable<Operation> items)
    {
        return JsonConvert.SerializeObject(items, Formatting.Indented);
    }
}