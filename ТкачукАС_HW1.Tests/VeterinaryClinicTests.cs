using Xunit;
using ТкачукАС_HW1.Interfaces;
using ТкачукАС_HW1.Services;
using ТкачукАС_HW1.Models;

namespace ТкачукАС_HW1.Tests
{
    public class VeterinaryClinicTests
    {
        [Fact]
        public void CheckHealth_WhenAnimalIsHealthy_ReturnsTrue()
        {
            // Arrange
            IVeterinaryClinic clinic = new VeterinaryClinic();
            var monkey = new Monkey("George", 5) { IsHealthy = true };
            // Act
            bool result = clinic.CheckHealth(monkey);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckHealth_WhenAnimalIsNotHealthy_ReturnsFalse()
        {
            // Arrange
            IVeterinaryClinic clinic = new VeterinaryClinic();
            var tiger = new Tiger("Sheru", 10) { IsHealthy = false };
            // Act
            bool result = clinic.CheckHealth(tiger);
            // Assert
            Assert.False(result);
        }
    }
}