using System.Collections.Generic;

namespace HSE_BANK.DataImportAndExport;

public abstract class GenericDataExporter<TDomain>
{
    public abstract string ExportData(IEnumerable<TDomain> items);
}