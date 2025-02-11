using Xunit;
using ТкачукАС_HW1.Models;

namespace ТкачукАС_HW1.Tests
{
    /// <summary>
    /// Тестирование животных
    /// </summary>
    public class AnimalTests
    {
        // Тесты для Monkey
        [Fact]
        public void MonkeyProperties_ShouldBeSetCorrectly()
        {
            // Arrange
            var monkey = new Monkey("George", 5);
            // Act & Assert
            Assert.Equal("George", monkey.Name);
            Assert.Equal(5, monkey.Food);
        }

        [Fact]
        public void MonkeyToString_ReturnsExpectedFormat()
        {
            // Arrange
            var monkey = new Monkey("George", 5);
            string expected = "George (Абизяна), Еда: 5 кг";
            // Act
            string actual = monkey.ToString();
            // Assert
            Assert.Equal(expected, actual);
        }

        // Тесты для Rabbit
        [Fact]
        public void RabbitProperties_ShouldBeSetCorrectly()
        {
            // Arrange
            var rabbit = new Rabbit("Bunny", 2, 8);
            // Act & Assert
            Assert.Equal("Bunny", rabbit.Name);
            Assert.Equal(2, rabbit.Food);
            Assert.Equal(8, rabbit.Kindness);
        }

        [Fact]
        public void RabbitToString_ReturnsExpectedFormat()
        {
            // Arrange
            var rabbit = new Rabbit("Bunny", 2, 8);
            string expected = "Bunny (Кролик), Еда: 2 кг, Доброта: 8";
            // Act
            string actual = rabbit.ToString();
            // Assert
            Assert.Equal(expected, actual);
        }

        // Тесты для Tiger
        [Fact]
        public void TigerProperties_ShouldBeSetCorrectly()
        {
            // Arrange
            var tiger = new Tiger("Sheru", 10);
            // Act & Assert
            Assert.Equal("Sheru", tiger.Name);
            Assert.Equal(10, tiger.Food);
        }

        [Fact]
        public void TigerToString_ReturnsExpectedFormat()
        {
            // Arrange
            var tiger = new Tiger("Sheru", 10);
            string expected = "Sheru (Тигр), Еда: 10 кг";
            // Act
            string actual = tiger.ToString();
            // Assert
            Assert.Equal(expected, actual);
        }

        // Тесты для Wolf
        [Fact]
        public void WolfProperties_ShouldBeSetCorrectly()
        {
            // Arrange
            var wolf = new Wolf("Luna", 7);
            // Act & Assert
            Assert.Equal("Luna", wolf.Name);
            Assert.Equal(7, wolf.Food);
        }

        [Fact]
        public void WolfToString_ReturnsExpectedFormat()
        {
            // Arrange
            var wolf = new Wolf("Luna", 7);
            string expected = "Luna (Волк), Еда: 7 кг";
            // Act
            string actual = wolf.ToString();
            // Assert
            Assert.Equal(expected, actual);
        }

        // Тесты для Herbo
        [Fact]
        public void HerboProperties_ShouldBeSetCorrectly()
        {
            // Arrange
            var herbo = new Herbo("Fluffy", 3, 9);
            // Act & Assert
            Assert.Equal("Fluffy", herbo.Name);
            Assert.Equal(3, herbo.Food);
            Assert.Equal(9, herbo.Kindness);
        }

        [Fact]
        public void HerboToString_ReturnsExpectedFormat()
        {
            // Arrange
            var herbo = new Herbo("Fluffy", 3, 9);
            string expected = "Fluffy (Травоядный), Еда: 3 кг, Доброта: 9";
            // Act
            string actual = herbo.ToString();
            // Assert
            Assert.Equal(expected, actual);
        }

        // Тесты для Predator
        [Fact]
        public void PredatorProperties_ShouldBeSetCorrectly()
        {
            // Arrange
            var predator = new Predator("Fang", 8);
            // Act & Assert
            Assert.Equal("Fang", predator.Name);
            Assert.Equal(8, predator.Food);
        }

        [Fact]
        public void PredatorToString_ReturnsExpectedFormat()
        {
            // Arrange
            var predator = new Predator("Fang", 8);
            string expected = "Fang (Хищник), Еда: 8 кг";
            // Act
            string actual = predator.ToString();
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
