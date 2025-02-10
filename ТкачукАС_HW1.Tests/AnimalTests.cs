using Xunit;
using ТкачукАС_HW1.Models;

namespace ТкачукАС_HW1.Tests
{
    public class AnimalTests
    {
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
    }
}