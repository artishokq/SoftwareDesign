using System.Collections.Generic;
using System.Globalization;
using System.Text;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.csv;

public class OperationCsvExportVisitor : GenericDataExporter<Operation>
{
    public override string ExportData(IEnumerable<Operation> items)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Type,BankAccountId,Amount,Date,CategoryId,Description");
        foreach (var op in items)
        {
            sb.AppendLine(
                $"{op.Type},{op.BankAccountId},{op.Amount.ToString(CultureInfo.InvariantCulture)},{op.Date.ToString("o")},{op.CategoryId},{op.Description}");
        }

        return sb.ToString();
    }
}