using NUnit.Framework;
using sebingel.sharpchievements;

namespace sharpchievements.UnitTest
{
    [TestFixture]
    public class AchievementManagerTests
    {
        [Test]
        public void Constructor_CreateInstance_InstanceCanBeCreated()
        {
            // Arrange
            AchievementManager am;

            // Act
            am = new AchievementManager();

            // Assert
            Assert.IsNotNull(am);
        }
    }
}