using System.Collections.Generic;
using HSE_BANK.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HSE_BANK.DataImportAndExport.yaml;

public class OperationYamlExportVisitor : GenericDataExporter<Operation>
{
    public override string ExportData(IEnumerable<Operation> items)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return serializer.Serialize(items);
    }
}