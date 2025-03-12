using System.Collections.Generic;
using System.Globalization;
using Xunit;
using Newtonsoft.Json;
using HSE_BANK.Domain;
using HSE_BANK.DataImportAndExport;
using HSE_BANK.DataImportAndExport.csv;
using HSE_BANK.DataImportAndExport.json;
using HSE_BANK.DataImportAndExport.yaml;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Tests;

public class DataImportExportTests
{
    #region Operation Tests

    [Fact]
    public void OperationJsonDataImporter_ParsesOperations_Correctly()
    {
        var sampleOps = new List<OperationDto>
        {
            new OperationDto
            {
                Type = OperationType.Income,
                BankAccountId = Guid.NewGuid(),
                Amount = 200,
                Date = DateTime.Now,
                CategoryId = Guid.NewGuid(),
                Description = "JSON Operation Test"
            }
        };
        string jsonContent = JsonConvert.SerializeObject(sampleOps, Formatting.Indented);
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, jsonContent);

        var importer = new OperationJsonDataImporter();
        var ops = importer.ImportData(tempFile);
        Assert.Single(ops);
        Assert.Equal(200, ops[0].Amount);
        Assert.Equal("JSON Operation Test", ops[0].Description);

        File.Delete(tempFile);
    }

    [Fact]
    public void OperationCsvDataImporter_ParsesOperations_Correctly()
    {
        var op = DomainFactory.CreateOperation(OperationType.Expense, Guid.NewGuid(), 150, DateTime.Now,
            Guid.NewGuid(), "CSV Operation Test");
        string dateStr = op.Date.ToString("o", CultureInfo.InvariantCulture);
        string csvContent =
            $"{op.Type},{op.BankAccountId},{op.Amount.ToString(CultureInfo.InvariantCulture)},{dateStr},{op.CategoryId},{op.Description}";
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, csvContent);

        var importer = new OperationCsvDataImporter();
        var ops = importer.ImportData(tempFile);
        Assert.Single(ops);
        Assert.Equal(150, ops[0].Amount);
        Assert.Equal("CSV Operation Test", ops[0].Description);

        File.Delete(tempFile);
    }

    #endregion

    #region BankAccount Tests

    [Fact]
    public void BankAccountJsonDataImporter_ParsesAccounts_Correctly()
    {
        var sampleAcc = new BankAccountDto
        {
            Id = Guid.NewGuid(),
            Name = "Test Account",
            Balance = 1500.50m
        };
        string jsonContent =
            JsonConvert.SerializeObject(new List<BankAccountDto> { sampleAcc }, Formatting.Indented);
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, jsonContent);

        var importer = new BankAccountJsonDataImporter();
        var accounts = importer.ImportData(tempFile);
        Assert.Single(accounts);
        Assert.Equal("Test Account", accounts[0].Name);
        Assert.Equal(1500.50m, accounts[0].Balance);

        File.Delete(tempFile);
    }

    [Fact]
    public void BankAccountCsvDataImporter_ParsesAccounts_Correctly()
    {
        var sampleId = Guid.NewGuid();
        string csvContent = $"Id,Name,Balance\r\n{sampleId},Test Account,1500.50";
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, csvContent);

        var importer = new BankAccountCsvDataImporter();
        var accounts = importer.ImportData(tempFile);
        Assert.Single(accounts);
        Assert.Equal("Test Account", accounts[0].Name);
        Assert.Equal(1500.50m, accounts[0].Balance);

        File.Delete(tempFile);
    }

    [Fact]
    public void BankAccountYamlDataImporter_ParsesAccounts_Correctly()
    {
        var sampleAcc = new BankAccountDto
        {
            Id = Guid.NewGuid(),
            Name = "Test Account",
            Balance = 1500.50m
        };
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        string yamlContent = serializer.Serialize(new List<BankAccountDto> { sampleAcc });
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, yamlContent);

        var importer = new BankAccountYamlDataImporter();
        var accounts = importer.ImportData(tempFile);
        Assert.Single(accounts);
        Assert.Equal("Test Account", accounts[0].Name);
        Assert.Equal(1500.50m, accounts[0].Balance);

        File.Delete(tempFile);
    }

    #endregion

    #region Category Tests

    [Fact]
    public void CategoryJsonDataImporter_ParsesCategories_Correctly()
    {
        var sampleCat = new CategoryDto
        {
            Id = Guid.NewGuid(),
            Name = "Salary",
            Type = CategoryType.Income
        };
        string jsonContent = JsonConvert.SerializeObject(new List<CategoryDto> { sampleCat }, Formatting.Indented);
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, jsonContent);

        var importer = new CategoryJsonDataImporter();
        var cats = importer.ImportData(tempFile);
        Assert.Single(cats);
        Assert.Equal("Salary", cats[0].Name);
        Assert.Equal(CategoryType.Income, cats[0].Type);

        File.Delete(tempFile);
    }

    [Fact]
    public void CategoryCsvDataImporter_ParsesCategories_Correctly()
    {
        var sampleId = Guid.NewGuid();
        string csvContent = $"Id,Name,Type\r\n{sampleId},Salary,Income";
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, csvContent);

        var importer = new CategoryCsvDataImporter();
        var cats = importer.ImportData(tempFile);
        Assert.Single(cats);
        Assert.Equal("Salary", cats[0].Name);
        Assert.Equal(CategoryType.Income, cats[0].Type);

        File.Delete(tempFile);
    }
    #endregion
}