using System.Collections.Generic;
using System.Text;
using HSE_BANK.Domain;

namespace HSE_BANK.DataImportAndExport.csv;

public class CategoryCsvExportVisitor : GenericDataExporter<Category>
{
    public override string ExportData(IEnumerable<Category> items)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Name,Type");
        foreach (var cat in items)
        {
            sb.AppendLine($"{cat.Id},{cat.Name},{cat.Type}");
        }

        return sb.ToString();
    }
}