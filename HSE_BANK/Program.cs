using HSE_BANK.Commands;
using HSE_BANK.Decorators;
using HSE_BANK.DataImportAndExport.json;
using HSE_BANK.DataImportAndExport.csv;
using HSE_BANK.DataImportAndExport.yaml;
using HSE_BANK.Domain;
using HSE_BANK.DataAccess;
using HSE_BANK.Facades;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HSE_BANK;

class Program
{
    static void Main(string[] args)
    {
        // Настройка DI-контейнера
        var serviceCollection = new ServiceCollection();

        // Регистрация слоев доступа
        serviceCollection.AddSingleton<IRepository<BankAccount>, BankAccountDataAccess>();
        serviceCollection.AddSingleton<IRepository<Category>, CategoryDataAccess>();
        serviceCollection.AddSingleton<IRepository<Operation>, OperationDataAccess>();

        // Регистрация фасадов
        serviceCollection.AddTransient<BankAccountFacade>();
        serviceCollection.AddTransient<CategoryFacade>();
        serviceCollection.AddTransient<OperationFacade>();
        serviceCollection.AddTransient<AnalyticsFacade>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Получение фасадов и репозиториев
        var accountFacade = serviceProvider.GetService<BankAccountFacade>();
        var categoryFacade = serviceProvider.GetService<CategoryFacade>();
        var operationFacade = serviceProvider.GetService<OperationFacade>();
        var analyticsFacade = serviceProvider.GetService<AnalyticsFacade>();

        var bankAccountRepo = serviceProvider.GetService<IRepository<BankAccount>>();
        var categoryRepo = serviceProvider.GetService<IRepository<Category>>();
        var operationRepo = serviceProvider.GetService<IRepository<Operation>>();

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("==== Меню Учет Финансов HSE BANK ====");
            Console.WriteLine("1. Создать счет");
            Console.WriteLine("2. Изменить счет");
            Console.WriteLine("3. Удалить счет");
            Console.WriteLine("4. Показать счета");
            Console.WriteLine("5. Создать категорию");
            Console.WriteLine("6. Изменить категорию");
            Console.WriteLine("7. Удалить категорию");
            Console.WriteLine("8. Показать категории");
            Console.WriteLine("9. Добавить операцию");
            Console.WriteLine("10. Удалить операцию");
            Console.WriteLine("11. Показать операции");
            Console.WriteLine("12. Показать разницу доходов и расходов за период");
            Console.WriteLine("13. Экспорт данных (JSON)");
            Console.WriteLine("14. Экспорт данных (CSV)");
            Console.WriteLine("15. Экспорт данных (YAML)");
            Console.WriteLine("16. Импорт данных (JSON)");
            Console.WriteLine("17. Импорт данных (CSV)");
            Console.WriteLine("18. Импорт данных (YAML)");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
            var input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                // Базовые операции (счета, категории, операции, аналитика)
                case "1":
                    Console.Write("Введите название счета: ");
                    var accName = Console.ReadLine();
                    Console.Write("Введите начальный баланс: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal initBalance))
                    {
                        var newAccount = accountFacade.CreateAccount(accName, initBalance);
                        Console.WriteLine(
                            $"Счет создан. ID: {newAccount.Id}, Название: {newAccount.Name}, Баланс: {newAccount.Balance}");
                    }
                    else Console.WriteLine("Неверный формат баланса.");

                    break;
                case "2":
                    Console.Write("Введите ID счета для изменения: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid accId))
                    {
                        Console.Write("Введите новое название счета: ");
                        var newName = Console.ReadLine();
                        try
                        {
                            accountFacade.EditAccount(accId, newName);
                            Console.WriteLine("Счет изменен.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                    else Console.WriteLine("Неверный формат ID.");

                    break;
                case "3":
                    Console.Write("Введите ID счета для удаления: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid accDelId))
                    {
                        try
                        {
                            accountFacade.DeleteAccount(accDelId);
                            Console.WriteLine("Счет удален.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                    else Console.WriteLine("Неверный формат ID.");

                    break;
                case "4":
                    var accounts = bankAccountRepo.GetAll().ToList();
                    if (accounts.Any())
                    {
                        Console.WriteLine("Счета:");
                        foreach (var acc in accounts)
                            Console.WriteLine($"ID: {acc.Id} | Название: {acc.Name} | Баланс: {acc.Balance}");
                    }
                    else Console.WriteLine("Счета отсутствуют.");

                    break;
                case "5":
                    Console.Write("Введите название категории: ");
                    var catName = Console.ReadLine();
                    Console.Write("Выберите тип категории (1 - Доход, 2 - Расход): ");
                    var catTypeInput = Console.ReadLine();
                    CategoryType catType = catTypeInput == "1" ? CategoryType.Income : CategoryType.Expense;
                    try
                    {
                        var newCat = categoryFacade.CreateCategory(catType, catName);
                        Console.WriteLine(
                            $"Категория создана. ID: {newCat.Id}, Название: {newCat.Name}, Тип: {newCat.Type}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }

                    break;
                case "6":
                    Console.Write("Введите ID категории для изменения: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid catId))
                    {
                        Console.Write("Введите новое название категории: ");
                        var newCatName = Console.ReadLine();
                        try
                        {
                            categoryFacade.EditCategory(catId, newCatName);
                            Console.WriteLine("Категория изменена.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                    else Console.WriteLine("Неверный формат ID.");

                    break;
                case "7":
                    Console.Write("Введите ID категории для удаления: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid catDelId))
                    {
                        try
                        {
                            categoryFacade.DeleteCategory(catDelId);
                            Console.WriteLine("Категория удалена.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                    else Console.WriteLine("Неверный формат ID.");

                    break;
                case "8":
                    var cats = categoryRepo.GetAll().ToList();
                    if (cats.Any())
                    {
                        Console.WriteLine("Категории:");
                        foreach (var cat in cats)
                            Console.WriteLine($"ID: {cat.Id} | Название: {cat.Name} | Тип: {cat.Type}");
                    }
                    else Console.WriteLine("Категории отсутствуют.");

                    break;
                case "9":
                    // Добавление операции с проверками
                    Console.Write("Введите тип операции (1 - Доход, 2 - Расход): ");
                    var opTypeInput = Console.ReadLine();
                    OperationType opType = opTypeInput == "1" ? OperationType.Income : OperationType.Expense;
                    Console.Write("Введите ID счета: ");
                    if (!Guid.TryParse(Console.ReadLine(), out Guid opAccId))
                    {
                        Console.WriteLine("Неверный формат ID счета.");
                        break;
                    }

                    Console.Write("Введите сумму операции: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal opAmount))
                    {
                        Console.WriteLine("Неверный формат суммы.");
                        break;
                    }

                    Console.Write("Введите дату операции (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime opDate))
                    {
                        Console.WriteLine("Неверный формат даты.");
                        break;
                    }

                    Console.Write("Введите ID категории: ");
                    if (!Guid.TryParse(Console.ReadLine(), out Guid opCatId))
                    {
                        Console.WriteLine("Неверный формат ID категории.");
                        break;
                    }

                    if (bankAccountRepo.GetById(opAccId) == null)
                    {
                        Console.WriteLine("Счет с таким ID не найден.");
                        break;
                    }

                    if (categoryRepo.GetById(opCatId) == null)
                    {
                        Console.WriteLine("Категория с таким ID не найдена.");
                        break;
                    }

                    var opCat = categoryRepo.GetById(opCatId);
                    if ((opType == OperationType.Income && opCat.Type != CategoryType.Income) ||
                        (opType == OperationType.Expense && opCat.Type != CategoryType.Expense))
                    {
                        Console.WriteLine("Ошибка: Тип операции и тип категории не совпадают.");
                        break;
                    }

                    Console.Write("Введите описание операции (необязательно): ");
                    var opDesc = Console.ReadLine();
                    try
                    {
                        var addOpCmd = new AddOperationCommand(operationFacade, opType, opAccId, opAmount, opDate,
                            opCatId, opDesc);
                        var timedCmd = new CommandTimerDecorator(addOpCmd);
                        timedCmd.Execute();
                        Console.WriteLine("Операция добавлена.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }

                    break;
                case "10":
                    Console.Write("Введите ID операции для удаления: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid opDelId))
                    {
                        try
                        {
                            operationFacade.DeleteOperation(opDelId);
                            Console.WriteLine("Операция удалена.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                    else Console.WriteLine("Неверный формат ID.");

                    break;
                case "11":
                    var ops = operationRepo.GetAll().ToList();
                    if (ops.Any())
                    {
                        Console.WriteLine("Операции:");
                        foreach (var op in ops)
                            Console.WriteLine(
                                $"ID: {op.Id} | Тип: {op.Type} | Счет: {op.BankAccountId} | Сумма: {op.Amount} | Дата: {op.Date} | Категория: {op.CategoryId} | Описание: {op.Description}");
                    }
                    else Console.WriteLine("Операции отсутствуют.");

                    break;
                case "12":
                    Console.Write("Введите начальную дату (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                    {
                        Console.WriteLine("Неверный формат даты.");
                        break;
                    }

                    Console.Write("Введите конечную дату (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                    {
                        Console.WriteLine("Неверный формат даты.");
                        break;
                    }

                    var netIncome = analyticsFacade.GetNetIncome(startDate, endDate);
                    Console.WriteLine($"Разница между доходами и расходами: {netIncome}");
                    break;

                // Экспорт / Импорт данных (единый пункт, затем выбор сущности)
                case "13":
                case "14":
                case "15":
                    // Экспорт данных: сначала спрашиваем формат (13-JSON, 14-CSV, 15-YAML)
                    Console.WriteLine("Экспорт данных. Выберите тип сущности для экспорта:");
                    Console.WriteLine("1. Операции");
                    Console.WriteLine("2. Счета");
                    Console.WriteLine("3. Категории");
                    Console.Write("Ваш выбор: ");
                    var expChoice = Console.ReadLine();
                    string exportOutput = string.Empty;
                    string exportPath = string.Empty;
                    if (input == "13")
                    {
                        Console.Write("Введите путь для экспорта (например, export_data.json): ");
                        exportPath = Console.ReadLine();
                        if (expChoice == "1")
                            exportOutput = new OperationJsonExportVisitor().ExportData(operationRepo.GetAll());
                        else if (expChoice == "2")
                            exportOutput = new BankAccountJsonExportVisitor().ExportData(bankAccountRepo.GetAll());
                        else if (expChoice == "3")
                            exportOutput = new CategoryJsonExportVisitor().ExportData(categoryRepo.GetAll());
                    }
                    else if (input == "14")
                    {
                        Console.Write("Введите путь для экспорта (например, export_data.csv): ");
                        exportPath = Console.ReadLine();
                        if (expChoice == "1")
                            exportOutput = new OperationCsvExportVisitor().ExportData(operationRepo.GetAll());
                        else if (expChoice == "2")
                            exportOutput = new BankAccountCsvExportVisitor().ExportData(bankAccountRepo.GetAll());
                        else if (expChoice == "3")
                            exportOutput = new CategoryCsvExportVisitor().ExportData(categoryRepo.GetAll());
                    }
                    else if (input == "15")
                    {
                        Console.Write("Введите путь для экспорта (например, export_data.yaml): ");
                        exportPath = Console.ReadLine();
                        if (expChoice == "1")
                            exportOutput = new OperationYamlExportVisitor().ExportData(operationRepo.GetAll());
                        else if (expChoice == "2")
                            exportOutput = new BankAccountYamlExportVisitor().ExportData(bankAccountRepo.GetAll());
                        else if (expChoice == "3")
                            exportOutput = new CategoryYamlExportVisitor().ExportData(categoryRepo.GetAll());
                    }

                    try
                    {
                        File.WriteAllText(exportPath, exportOutput);
                        Console.WriteLine($"Данные успешно экспортированы в файл {exportPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка экспорта: {ex.Message}");
                    }

                    break;
                case "16":
                case "17":
                case "18":
                    // Импорт данных: сначала спрашиваем тип сущности для импорта
                    Console.WriteLine("Импорт данных. Выберите тип сущности для импорта:");
                    Console.WriteLine("1. Операции");
                    Console.WriteLine("2. Счета");
                    Console.WriteLine("3. Категории");
                    Console.Write("Ваш выбор: ");
                    var impChoice = Console.ReadLine();
                    Console.Write("Введите путь к файлу для импорта: ");
                    var importPath = Console.ReadLine();
                    try
                    {
                        if (input == "16") // JSON
                        {
                            if (impChoice == "1")
                            {
                                var importedOps = new OperationJsonDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var op in importedOps)
                                {
                                    // При импорте операции проверяем существование счета и категории
                                    if (bankAccountRepo.GetById(op.BankAccountId) != null &&
                                        categoryRepo.GetById(op.CategoryId) != null)
                                    {
                                        operationRepo.Add(op);
                                        count++;
                                    }
                                }

                                Console.WriteLine($"Импортировано {count} операций из JSON.");
                            }
                            else if (impChoice == "2")
                            {
                                var importedAcc = new BankAccountJsonDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var acc in importedAcc)
                                {
                                    bankAccountRepo.Add(acc);
                                    count++;
                                }

                                Console.WriteLine($"Импортировано {count} счетов из JSON.");
                            }
                            else if (impChoice == "3")
                            {
                                var importedCat = new CategoryJsonDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var cat in importedCat)
                                {
                                    categoryRepo.Add(cat);
                                    count++;
                                }

                                Console.WriteLine($"Импортировано {count} категорий из JSON.");
                            }
                        }
                        else if (input == "17") // CSV
                        {
                            if (impChoice == "1")
                            {
                                var importedOps = new OperationCsvDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var op in importedOps)
                                {
                                    if (bankAccountRepo.GetById(op.BankAccountId) != null &&
                                        categoryRepo.GetById(op.CategoryId) != null)
                                    {
                                        operationRepo.Add(op);
                                        count++;
                                    }
                                }

                                Console.WriteLine($"Импортировано {count} операций из CSV.");
                            }
                            else if (impChoice == "2")
                            {
                                var importedAcc = new BankAccountCsvDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var acc in importedAcc)
                                {
                                    bankAccountRepo.Add(acc);
                                    count++;
                                }

                                Console.WriteLine($"Импортировано {count} счетов из CSV.");
                            }
                            else if (impChoice == "3")
                            {
                                var importedCat = new CategoryCsvDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var cat in importedCat)
                                {
                                    categoryRepo.Add(cat);
                                    count++;
                                }

                                Console.WriteLine($"Импортировано {count} категорий из CSV.");
                            }
                        }
                        else if (input == "18") // YAML
                        {
                            if (impChoice == "1")
                            {
                                var importedOps = new OperationYamlDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var op in importedOps)
                                {
                                    if (bankAccountRepo.GetById(op.BankAccountId) != null &&
                                        categoryRepo.GetById(op.CategoryId) != null)
                                    {
                                        operationRepo.Add(op);
                                        count++;
                                    }
                                }

                                Console.WriteLine($"Импортировано {count} операций из YAML.");
                            }
                            else if (impChoice == "2")
                            {
                                var importedAcc = new BankAccountYamlDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var acc in importedAcc)
                                {
                                    bankAccountRepo.Add(acc);
                                    count++;
                                }

                                Console.WriteLine($"Импортировано {count} счетов из YAML.");
                            }
                            else if (impChoice == "3")
                            {
                                var importedCat = new CategoryYamlDataImporter().ImportData(importPath);
                                int count = 0;
                                foreach (var cat in importedCat)
                                {
                                    categoryRepo.Add(cat);
                                    count++;
                                }

                                Console.WriteLine($"Импортировано {count} категорий из YAML.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка импорта: {ex.Message}");
                    }

                    break;
                case "0":
                    exit = true;
                    continue;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}