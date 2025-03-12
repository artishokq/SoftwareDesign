using Newtonsoft.Json;
using System.Collections.Generic;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.json;

public class CategoryJsonExportVisitor : GenericDataExporter<Category>
{
    public override string ExportData(IEnumerable<Category> items)
    {
        return JsonConvert.SerializeObject(items, Formatting.Indented);
    }
}