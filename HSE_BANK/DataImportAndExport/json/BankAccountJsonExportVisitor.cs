using Newtonsoft.Json;
using System.Collections.Generic;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.json;

public class BankAccountJsonExportVisitor : GenericDataExporter<BankAccount>
{
    public override string ExportData(IEnumerable<BankAccount> items)
    {
        return JsonConvert.SerializeObject(items, Formatting.Indented);
    }
}