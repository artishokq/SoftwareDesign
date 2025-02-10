using System;
using ТкачукАС_HW1.Interfaces;
using ТкачукАС_HW1.Models;
using ТкачукАС_HW1.Things;

namespace ТкачукАС_HW1
{
    /// <summary>
    /// Класс действий
    /// </summary>
    public class Actions
    {
        /// <summary>
        /// Метод для добавления нового животного.
        /// </summary>
        public static void AddAnimal(IZoo zoo)
        {
            // Запрашиваем тип животного с проверкой корректности ввода
            string choice;
            while (true)
            {
                Console.WriteLine("\nВыберите тип животного:");
                Console.WriteLine("1. Обезьяна");
                Console.WriteLine("2. Кролик");
                Console.WriteLine("3. Тигр");
                Console.WriteLine("4. Волк");
                Console.Write("Ваш выбор: ");
                choice = Console.ReadLine()?.Trim();
                if (choice == "1" || choice == "2" || choice == "3" || choice == "4")
                    break;
                Console.WriteLine("Неверный выбор. Пожалуйста, введите 1, 2, 3 или 4.");
            }

            // Запрос имени с проверкой, что оно не пустое
            string name;
            while (true)
            {
                Console.Write("Введите имя: ");
                name = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(name))
                    break;
                Console.WriteLine("Имя не может быть пустым. Попробуйте снова.");
            }

            // Запрос количества еды с проверкой корректности ввода
            int food;
            while (true)
            {
                Console.Write("Введите потребление еды (кг/день): ");
                if (int.TryParse(Console.ReadLine(), out food))
                    break;
                Console.WriteLine("Неверный формат количества еды. Попробуйте снова.");
            }

            // Запрос состояния здоровья с проверкой ввода
            bool isHealthy;
            while (true)
            {
                Console.Write("Животное здорово? (y/n): ");
                string healthyInput = Console.ReadLine()?.Trim().ToLower();
                if (healthyInput == "y")
                {
                    isHealthy = true;
                    break;
                }
                else if (healthyInput == "n")
                {
                    isHealthy = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Введите 'y' для Да или 'n' для Нет.");
                }
            }

            Animal animal = null;
            switch (choice)
            {
                case "1":
                    animal = new Monkey(name, food);
                    break;
                case "2":
                    // Для кролика требуется дополнительно уровень доброты
                    int kindness;
                    while (true)
                    {
                        Console.Write("Введите уровень доброты (0-10): ");
                        if (int.TryParse(Console.ReadLine(), out kindness))
                        {
                            if (kindness >= 0 && kindness <= 10)
                                break;
                            else
                                Console.WriteLine("Уровень доброты должен быть в диапазоне от 0 до 10. Попробуйте снова.");
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат уровня доброты. Попробуйте снова.");
                        }
                    }
                    animal = new Rabbit(name, food, kindness);
                    break;
                case "3":
                    animal = new Tiger(name, food);
                    break;
                case "4":
                    animal = new Wolf(name, food);
                    break;
                default:
                    // На этом этапе этот блок никогда не выполнится, поскольку выбор проверен циклом выше.
                    Console.WriteLine("Неверно выбран тип животного.");
                    return;
            }
            animal.IsHealthy = isHealthy;
            zoo.AddAnimal(animal);
        }

        /// <summary>
        /// Метод для добавления нового инвентаризуемого объекта.
        /// </summary>
        public static void AddThing(IZoo zoo)
        {
            // Запрашиваем тип вещи с проверкой ввода
            string choice;
            while (true)
            {
                Console.WriteLine("\nВыберите тип вещи:");
                Console.WriteLine("1. Стол");
                Console.WriteLine("2. Компьютер");
                Console.Write("Ваш выбор: ");
                choice = Console.ReadLine()?.Trim();
                if (choice == "1" || choice == "2")
                    break;
                Console.WriteLine("Неверный выбор. Введите 1 или 2.");
            }

            // Запрос инвентарного номера с проверкой ввода
            int number;
            while (true)
            {
                Console.Write("Введите инвентаризационный номер: ");
                if (int.TryParse(Console.ReadLine(), out number))
                    break;
                Console.WriteLine("Неверный формат номера. Попробуйте снова.");
            }

            // Запрос наименования с проверкой, что оно не пустое
            string name;
            while (true)
            {
                Console.Write("Введите наименование: ");
                name = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(name))
                    break;
                Console.WriteLine("Наименование не может быть пустым. Попробуйте снова.");
            }

            Thing thing = null;
            switch (choice)
            {
                case "1":
                    thing = new Table(number, name);
                    break;
                case "2":
                    thing = new Computer(number, name);
                    break;
                default:
                    // Этот блок никогда не выполнится, так как выбор уже проверен.
                    Console.WriteLine("Неверно выбран тип вещи.");
                    return;
            }
            zoo.AddThing(thing);
        }

        /// <summary>
        /// Вывод списка животных.
        /// </summary>
        public static void ShowAnimals(IZoo zoo)
        {
            Console.WriteLine("\nСписок животных в зоопарке:");
            foreach (var animal in zoo.GetAnimals())
            {
                Console.WriteLine(animal);
            }
        }

        /// <summary>
        /// Вывод списка животных, пригодных для контактного зоопарка.
        /// </summary>
        public static void ShowInteractiveAnimals(IZoo zoo)
        {
            Console.WriteLine("\nЖивотные для контактного зоопарка (травоядные с добротой > 5):");
            foreach (var animal in zoo.GetInteractiveAnimals())
            {
                Console.WriteLine(animal);
            }
        }

        /// <summary>
        /// Вывод списка всех инвентаризуемых объектов.
        /// </summary>
        public static void ShowInventoryItems(IZoo zoo)
        {
            Console.WriteLine("\nИнвентаризационные объекты:");
            foreach (var item in zoo.GetInventoryItems())
            {
                Console.WriteLine(item);
            }
        }
    }
}
