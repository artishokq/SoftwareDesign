using Xunit;
using ТкачукАС_HW1.Things;

namespace ТкачукАС_HW1.Tests
{
    public class ThingTests
    {
        [Fact]
        public void TableProperties_ShouldBeSetCorrectly()
        {
            // Arrange
            var table = new Table(101, "Office Table");
            // Act & Assert
            Assert.Equal(101, table.Number);
            Assert.Equal("Office Table", table.Name);
        }

        [Fact]
        public void TableToString_ReturnsExpectedFormat()
        {
            // Arrange
            var table = new Table(101, "Office Table");
            string expected = "Office Table (Стол) - Инвентарь #101";
            // Act
            string actual = table.ToString();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComputerProperties_ShouldBeSetCorrectly()
        {
            // Arrange
            var computer = new Computer(202, "Laptop");
            // Act & Assert
            Assert.Equal(202, computer.Number);
            Assert.Equal("Laptop", computer.Name);
        }

        [Fact]
        public void ComputerToString_ReturnsExpectedFormat()
        {
            // Arrange
            var computer = new Computer(202, "Laptop");
            string expected = "Laptop (Компьютер) - Инвентарь #202";
            // Act
            string actual = computer.ToString();
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}