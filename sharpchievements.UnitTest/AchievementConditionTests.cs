using NUnit.Framework;
using sebingel.sharpchievements;

namespace sharpchievements.UnitTest
{
    [TestFixture]
    public class AchievementConditionTests
    {
        [Test]
        public void Constructor_CreateInstance_CanCreateInstance()
        {
            // Arrange
            AchievementCondition ac;

            // Act
            ac = new AchievementCondition("", "", 0);

            // Assert
            Assert.IsNotNull(ac);
        }

        [Test]
        public void MakeProgress_GivenAchievementConditionThatNeedsACountOf2_Is50PercentCompleted()
        {
            // Arrange
            AchievementCondition ac = new AchievementCondition("", "", 2);

            IAchievementCondition reportedAchievementConditionProgressChanged = null;
            int reportedProgressCount = 0;
            ac.ProgressChanged += delegate(IAchievementCondition iac, AchievementConditionProgressChangedArgs args)
            {
                reportedAchievementConditionProgressChanged = iac;
                reportedProgressCount = args.ProgressCount;
            };

            IAchievementCondition reportedAchievementConditionCompleted = null;
            ac.ConditionCompleted +=
                delegate(IAchievementCondition iac) { reportedAchievementConditionCompleted = iac; };

            // Act
            ac.MakeProgress();

            // Assert
            Assert.AreEqual(50, ac.Progress);
            Assert.AreEqual(1, ac.ProgressCount);
            Assert.AreEqual(ac, reportedAchievementConditionProgressChanged);
            Assert.AreEqual(1, reportedProgressCount);
            Assert.IsNull(reportedAchievementConditionCompleted);
        }

        [Test]
        public void MakeProgress_GivenAchievementConditionThatNeedsACountOf1_IsCompleted()
        {
            // Arrange
            AchievementCondition ac = new AchievementCondition("", "", 1);

            IAchievementCondition reportedAchievementConditionProgressChanged = null;
            int reportedProgressCount = 0;
            ac.ProgressChanged += delegate(IAchievementCondition iac, AchievementConditionProgressChangedArgs args)
            {
                reportedAchievementConditionProgressChanged = iac;
                reportedProgressCount = args.ProgressCount;
            };

            IAchievementCondition reportedAchievementConditionCompleted = null;
            ac.ConditionCompleted +=
                delegate(IAchievementCondition iac) { reportedAchievementConditionCompleted = iac; };

            // Act
            ac.MakeProgress();

            // Assert
            Assert.AreEqual(100, ac.Progress);
            Assert.AreEqual(1, ac.ProgressCount);
            Assert.AreEqual(ac, reportedAchievementConditionProgressChanged);
            Assert.AreEqual(1, reportedProgressCount);
            Assert.AreEqual(ac, reportedAchievementConditionCompleted);
        }
    }
}