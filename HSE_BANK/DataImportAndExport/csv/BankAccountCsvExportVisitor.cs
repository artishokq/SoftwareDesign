using System.Collections.Generic;
using System.Globalization;
using System.Text;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.csv;

public class BankAccountCsvExportVisitor : GenericDataExporter<BankAccount>
{
    public override string ExportData(IEnumerable<BankAccount> items)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Name,Balance");
        foreach (var acc in items)
        {
            sb.AppendLine($"{acc.Id},{acc.Name},{acc.Balance.ToString(CultureInfo.InvariantCulture)}");
        }

        return sb.ToString();
    }
}