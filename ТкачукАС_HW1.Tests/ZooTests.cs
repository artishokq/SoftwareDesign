using Xunit;
using System.Linq;
using ТкачукАС_HW1.Interfaces;
using ТкачукАС_HW1.Services;
using ТкачукАС_HW1.Models;
using ТкачукАС_HW1.Things;

namespace ТкачукАС_HW1.Tests
{
    /// <summary>
    /// Тестирование зоопарка
    /// </summary>
    public class ZooTests
    {
        [Fact]
        public void AddAnimal_HealthyAnimal_IsAdded()
        {
            // Arrange
            IVeterinaryClinic clinic = new VeterinaryClinic();
            IZoo zoo = new Zoo(clinic);
            var rabbit = new Rabbit("Bunny", 2, 8) { IsHealthy = true };

            // Act
            zoo.AddAnimal(rabbit);
            var animals = zoo.GetAnimals();

            // Assert
            Assert.Contains(rabbit, animals);
        }

        [Fact]
        public void AddAnimal_UnhealthyAnimal_IsNotAdded()
        {
            // Arrange
            IVeterinaryClinic clinic = new VeterinaryClinic();
            IZoo zoo = new Zoo(clinic);
            var tiger = new Tiger("Sheru", 10) { IsHealthy = false };

            // Act
            zoo.AddAnimal(tiger);
            var animals = zoo.GetAnimals();

            // Assert
            Assert.DoesNotContain(tiger, animals);
        }

        [Fact]
        public void TotalFoodConsumption_ReturnsSumOfFoodOfAllAnimals()
        {
            // Arrange
            IVeterinaryClinic clinic = new VeterinaryClinic();
            IZoo zoo = new Zoo(clinic);
            var monkey = new Monkey("George", 3) { IsHealthy = true };
            var wolf = new Wolf("Luna", 5) { IsHealthy = true };

            zoo.AddAnimal(monkey);
            zoo.AddAnimal(wolf);

            // Act
            int totalFood = zoo.TotalFoodConsumption;

            // Assert
            Assert.Equal(8, totalFood);
        }

        [Fact]
        public void GetInteractiveAnimals_ReturnsOnlyHerboWithHighKindness()
        {
            // Arrange
            IVeterinaryClinic clinic = new VeterinaryClinic();
            IZoo zoo = new Zoo(clinic);
            var rabbit1 = new Rabbit("Fluffy", 2, 6) { IsHealthy = true };
            var rabbit2 = new Rabbit("Snowball", 2, 4) { IsHealthy = true };
            var tiger = new Tiger("Tiger", 8) { IsHealthy = true };

            zoo.AddAnimal(rabbit1);
            zoo.AddAnimal(rabbit2);
            zoo.AddAnimal(tiger);

            // Act
            var interactiveAnimals = zoo.GetInteractiveAnimals();

            // Assert
            Assert.Single(interactiveAnimals);
            Assert.Contains(rabbit1, interactiveAnimals);
        }

        [Fact]
        public void GetInventoryItems_ReturnsAllThings()
        {
            // Arrange
            IVeterinaryClinic clinic = new VeterinaryClinic();
            IZoo zoo = new Zoo(clinic);
            var table = new Table(101, "Office Table");
            var computer = new Computer(202, "Laptop");

            zoo.AddThing(table);
            zoo.AddThing(computer);

            // Act
            var inventoryItems = zoo.GetInventoryItems().ToList();

            // Assert
            Assert.Equal(2, inventoryItems.Count);
            Assert.Contains(table, inventoryItems);
            Assert.Contains(computer, inventoryItems);
        }
    }
}
