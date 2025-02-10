using System;
using Microsoft.Extensions.DependencyInjection;
using ТкачукАС_HW1.Interfaces;
using ТкачукАС_HW1.Services;

namespace ТкачукАС_HW1;

internal class Program
{
    static void Main()
    {
        // Настройка DI-контейнера
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IVeterinaryClinic, VeterinaryClinic>()
            .AddSingleton<IZoo, Zoo>()
            .BuildServiceProvider();

        // Получаем экземпляр зоопарка через DI
        var zoo = serviceProvider.GetService<IZoo>();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("=== Система учёта животных Московского зоопарка ===");
            Console.WriteLine("1. Добавить новое животное");
            Console.WriteLine("2. Добавить новый инвентаризационный объект (вещь)");
            Console.WriteLine("3. Вывести список животных");
            Console.WriteLine("4. Вывести суммарное потребление еды (кг/день)");
            Console.WriteLine("5. Вывести список животных для контактного зоопарка");
            Console.WriteLine("6. Вывести список инвентаризационных объектов");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Actions.AddAnimal(zoo);
                    break;
                case "2":
                    Actions.AddThing(zoo);
                    break;
                case "3":
                    Actions.ShowAnimals(zoo);
                    break;
                case "4":
                    Console.WriteLine($"Общее потребление еды: {zoo.TotalFoodConsumption} кг/день");
                    break;
                case "5":
                    Actions.ShowInteractiveAnimals(zoo);
                    break;
                case "6":
                    Actions.ShowInventoryItems(zoo);
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверная опция. Попробуйте снова.");
                    break;
            }
        }
    }
}