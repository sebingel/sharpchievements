using Moq;
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

        [Test]
        public void RegisterAchievementCondition_GivenANewAchievementCondition_AddsTheAchievementCondition()
        {
            // Arrange
            AchievementManager am = new AchievementManager();

            string key = "key";
            Mock<AchievementCondition> iAchievmentConditionMock = new Mock<AchievementCondition>();
            iAchievmentConditionMock.SetupGet(x => x.UniqueId).Returns("uniqueId");
            iAchievmentConditionMock.SetupGet(x => x.AchievementConditionKey).Returns(key);

            // Act
            am.RegisterAchievementCondition(iAchievmentConditionMock.Object);
            am.ReportProgress(key);

            // Assert
            iAchievmentConditionMock.Verify(x => x.MakeProgress(), Times.Once);
        }
    }
}