using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using sebingel.sharpchievements;

namespace sharpchievements.UnitTest
{
    [TestFixture]
    public class AchievementTests
    {
        [Test]
        public void Constructor_GivenAllNecessaryParameters_CanCreateInstance()
        {
            // Arrange
            Achievement a;

            // Act
            a = new Achievement("uniqueId", "titel", "description", new Mock<IAchievementCondition>().Object);

            // Assert
            Assert.IsNotNull(a);
        }

        [Test]
        public void Constructor2_GivenAllNecessaryParameters_CanCreateInstance()
        {
            // Arrange
            Achievement a;

            // Act
            a = new Achievement("uniqueId", "titel", "description",
                new List<IAchievementCondition> { new Mock<IAchievementCondition>().Object });

            // Assert
            Assert.IsNotNull(a);
        }

        [TestCase(null, "titel", "description", "uniqueId", typeof (ArgumentOutOfRangeException))]
        [TestCase("", "titel", "description", "uniqueId", typeof (ArgumentOutOfRangeException))]
        [TestCase("   ", "titel", "description", "uniqueId", typeof (ArgumentOutOfRangeException))]
        [TestCase("uniqueId", null, "description", "titel", typeof (ArgumentOutOfRangeException))]
        [TestCase("uniqueId", "", "description", "titel", typeof (ArgumentOutOfRangeException))]
        [TestCase("uniqueId", "   ", "description", "titel", typeof (ArgumentOutOfRangeException))]
        [TestCase("uniqueId", "titel", null, "description", typeof (ArgumentOutOfRangeException))]
        [TestCase("uniqueId", "titel", "", "description", typeof (ArgumentOutOfRangeException))]
        [TestCase("uniqueId", "titel", "   ", "description", typeof (ArgumentOutOfRangeException))]
        public void Constructor_GivenEmptyOrNullParameters_ShouldThrowException(string uniqueId, string titel,
            string description, string expectedArgument, Type expectedExceptionType)
        {
            // Arrange
            // Nothing to arrange here

            // Act
            try
            {
                new Achievement(uniqueId, titel, description, new Mock<IAchievementCondition>().Object);
                Assert.Fail("Should throw Exception");
            }
            catch (Exception e)
            {
                // Assert
                ArgumentNullException argumentNullException = e as ArgumentNullException;
                if (argumentNullException != null)
                {
                    Assert.AreEqual(typeof (ArgumentNullException), expectedExceptionType);
                    Assert.AreEqual(expectedArgument, argumentNullException.ParamName);
                    return;
                }

                ArgumentOutOfRangeException argumentOutOfRangeException = e as ArgumentOutOfRangeException;
                if (argumentOutOfRangeException != null)
                {
                    Assert.AreEqual(typeof (ArgumentOutOfRangeException), expectedExceptionType);
                    Assert.AreEqual(expectedArgument, argumentOutOfRangeException.ParamName);
                    return;
                }

                ArgumentException argumentException = e as ArgumentException;
                if (argumentException != null)
                {
                    Assert.AreEqual(typeof (ArgumentException), expectedExceptionType);
                    Assert.AreEqual(expectedArgument, argumentException.ParamName);
                    return;
                }

                Assert.Fail("No matching Exceptiontype found");
            }
        }

        [Test]
        public void Constructor_GivenIAchievementConditionNull_ShouldThrowException()
        {
            // Arrange
            IAchievementCondition achievementCondition = null;

            // Act
            try
            {
                new Achievement("uniqueId", "titel", "description", achievementCondition);
                Assert.Fail("Soll Exception werfen.");
            }
            catch (Exception e)
            {
                // Assert
                ArgumentNullException ane = e as ArgumentNullException;
                Assert.IsNotNull(ane);
                Assert.AreEqual("condition", ane.ParamName);
            }
        }

        [Test]
        public void Constructor_GivenListIAchievementConditionNull_ShouldThrowException()
        {
            // Arrange
            List<IAchievementCondition> achievementConditions = null;

            // Act
            try
            {
                new Achievement("uniqueId", "titel", "description", achievementConditions);
                Assert.Fail("Soll Exception werfen.");
            }
            catch (Exception e)
            {
                // Assert
                ArgumentNullException ane = e as ArgumentNullException;
                Assert.IsNotNull(ane);
                Assert.AreEqual("conditions", ane.ParamName);
            }
        }

        [Test]
        public void Constructor_GivenMultipleAchievementConsitionsWithSameUniqueId_ShouldOnlyAddOne()
        {
            // Arrange
            Mock<IAchievementCondition> acMock1 = new Mock<IAchievementCondition>();
            acMock1.SetupGet(x => x.UniqueId).Returns("UID1");

            Mock<IAchievementCondition> acMock2 = new Mock<IAchievementCondition>();
            acMock2.SetupGet(x => x.UniqueId).Returns("UID1");

            Mock<IAchievementCondition> acMock3 = new Mock<IAchievementCondition>();
            acMock3.SetupGet(x => x.UniqueId).Returns("UID1");

            // Act
            Achievement achievement = new Achievement("uid", "title", "desc",
                new List<IAchievementCondition> { acMock1.Object, acMock2.Object, acMock3.Object });

            // Assert
            Assert.AreEqual(1, achievement.Conditions.Count());
        }

        [Test]
        public void CheckUnlockStatus_EveryIAchievementConditionIsUnlocked_ShouldFireAchievementCompletedEvent()
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");

            Mock<IAchievementCondition> achievementConditionMock2 = new Mock<IAchievementCondition>();
            achievementConditionMock2.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock2.SetupGet(x => x.UniqueId).Returns("ac2");

            Mock<IAchievementCondition> achievementConditionMock3 = new Mock<IAchievementCondition>();
            achievementConditionMock3.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock3.SetupGet(x => x.UniqueId).Returns("ac3");

            List<IAchievementCondition> achievementConditions = new List<IAchievementCondition>
            {
                achievementConditionMock1.Object,
                achievementConditionMock2.Object,
                achievementConditionMock3.Object
            };
            Achievement achievement = new Achievement("uniqueId", "titel", "description", achievementConditions);

            Achievement reportedAchievement = null;
            achievement.AchievementCompleted += delegate(Achievement a) { reportedAchievement = a; };

            // Act
            achievement.CheckUnlockStatus();

            // Assert
            Assert.AreEqual(achievement, reportedAchievement);
            Assert.IsTrue(achievement.Unlocked);
        }

        [Test]
        public void CheckUnlockStatus_NotEveryIAchievementConditionIsUnlocked_ShouldNotFireAchievementCompletedEvent()
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");

            Mock<IAchievementCondition> achievementConditionMock2 = new Mock<IAchievementCondition>();
            achievementConditionMock2.SetupGet(x => x.Unlocked).Returns(false);
            achievementConditionMock2.SetupGet(x => x.UniqueId).Returns("ac2");

            Mock<IAchievementCondition> achievementConditionMock3 = new Mock<IAchievementCondition>();
            achievementConditionMock3.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock3.SetupGet(x => x.UniqueId).Returns("ac3");

            List<IAchievementCondition> achievementConditions = new List<IAchievementCondition>
            {
                achievementConditionMock1.Object,
                achievementConditionMock2.Object,
                achievementConditionMock3.Object
            };
            Achievement achievement = new Achievement("uniqueId", "titel", "description", achievementConditions);

            Achievement reportedAchievement = null;
            achievement.AchievementCompleted += delegate(Achievement a) { reportedAchievement = a; };

            // Act
            achievement.CheckUnlockStatus();

            // Assert
            Assert.IsNull(reportedAchievement);
            Assert.IsFalse(achievement.Unlocked);
        }

        [Test]
        public void CheckUnlockStatus_AchievementIsAlreadyUnlocked_ShouldNotFireAchievementCompletedEvent()
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");

            Mock<IAchievementCondition> achievementConditionMock2 = new Mock<IAchievementCondition>();
            achievementConditionMock2.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock2.SetupGet(x => x.UniqueId).Returns("ac2");

            Mock<IAchievementCondition> achievementConditionMock3 = new Mock<IAchievementCondition>();
            achievementConditionMock3.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock3.SetupGet(x => x.UniqueId).Returns("ac3");

            List<IAchievementCondition> achievementConditions = new List<IAchievementCondition>
            {
                achievementConditionMock1.Object,
                achievementConditionMock2.Object,
                achievementConditionMock3.Object
            };
            Achievement achievement = new Achievement("uniqueId", "titel", "description", achievementConditions);

            List<Achievement> reportedAchievements = new List<Achievement>();
            achievement.AchievementCompleted += delegate(Achievement a) { reportedAchievements.Add(a); };

            // Act
            achievement.CheckUnlockStatus();
            achievement.CheckUnlockStatus();

            // Assert
            Assert.AreEqual(1, reportedAchievements.Count);
            Assert.IsTrue(achievement.Unlocked);
        }

        [Test]
        public void ConditionCompleted_RaiseEventOnOnlyConditionAndConditionIsUnlocked_UnlockAchievement()
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");

            IAchievementCondition ac = achievementConditionMock1.Object;
            Achievement achievement = new Achievement("uniqueId", "titel", "description", ac);

            List<Achievement> reportedAchievements = new List<Achievement>();
            achievement.AchievementCompleted += delegate(Achievement a) { reportedAchievements.Add(a); };

            // Act
            achievementConditionMock1.Raise(x => x.ConditionCompleted += null, ac);

            // Assert
            Assert.AreEqual(1, reportedAchievements.Count);
            Assert.IsTrue(achievement.Unlocked);
        }

        [Test]
        public void
            ConditionCompleted_RaiseEventOnOneOfManyConditionsAndNotAllConditionsAreUnlocked_DoNotUnlockAchievement()
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");

            Mock<IAchievementCondition> achievementConditionMock2 = new Mock<IAchievementCondition>();
            achievementConditionMock2.SetupGet(x => x.Unlocked).Returns(false);
            achievementConditionMock2.SetupGet(x => x.UniqueId).Returns("ac2");

            Mock<IAchievementCondition> achievementConditionMock3 = new Mock<IAchievementCondition>();
            achievementConditionMock3.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock3.SetupGet(x => x.UniqueId).Returns("ac3");

            List<IAchievementCondition> achievementConditions = new List<IAchievementCondition>
            {
                achievementConditionMock1.Object,
                achievementConditionMock2.Object,
                achievementConditionMock3.Object
            };

            Achievement achievement = new Achievement("uniqueId", "titel", "description", achievementConditions);

            List<Achievement> reportedAchievements = new List<Achievement>();
            achievement.AchievementCompleted += delegate(Achievement a) { reportedAchievements.Add(a); };

            // Act
            achievementConditionMock1.Raise(x => x.ConditionCompleted += null, achievementConditions[0]);

            // Assert
            Assert.AreEqual(0, reportedAchievements.Count);
            Assert.IsFalse(achievement.Unlocked);
        }

        [Test]
        public void ConditionCompleted_RaiseEventOnOneOfManyConditionsAndAllConditionsAreUnlocked_UnlockAchievement()
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");

            Mock<IAchievementCondition> achievementConditionMock2 = new Mock<IAchievementCondition>();
            achievementConditionMock2.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock2.SetupGet(x => x.UniqueId).Returns("ac2");

            Mock<IAchievementCondition> achievementConditionMock3 = new Mock<IAchievementCondition>();
            achievementConditionMock3.SetupGet(x => x.Unlocked).Returns(true);
            achievementConditionMock3.SetupGet(x => x.UniqueId).Returns("ac3");

            List<IAchievementCondition> achievementConditions = new List<IAchievementCondition>
            {
                achievementConditionMock1.Object,
                achievementConditionMock2.Object,
                achievementConditionMock3.Object
            };

            Achievement achievement = new Achievement("uniqueId", "titel", "description", achievementConditions);

            List<Achievement> reportedAchievements = new List<Achievement>();
            achievement.AchievementCompleted += delegate(Achievement a) { reportedAchievements.Add(a); };

            // Act
            achievementConditionMock1.Raise(x => x.ConditionCompleted += null, achievementConditions[0]);

            // Assert
            Assert.AreEqual(1, reportedAchievements.Count);
            Assert.IsTrue(achievement.Unlocked);
        }

        [TestCase(50, 25)]
        [TestCase(100, 50)]
        [TestCase(1, 0)]
        [TestCase(2, 1)]
        [TestCase(99, 49)]
        public void ConditionProgressChanged_GivenSomeProgress_CalculatesProgressAndFiresProgressChangedEvent(
            int conditionProgress, int expectedAchievementProgress)
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");
            achievementConditionMock1.SetupGet(x => x.Progress).Returns(0);

            Mock<IAchievementCondition> achievementConditionMock2 = new Mock<IAchievementCondition>();
            achievementConditionMock2.SetupGet(x => x.UniqueId).Returns("ac2");
            achievementConditionMock2.SetupGet(x => x.Progress).Returns(conditionProgress);

            IAchievementCondition ac1 = achievementConditionMock1.Object;
            IAchievementCondition ac2 = achievementConditionMock2.Object;
            List<IAchievementCondition> achievementConditions = new List<IAchievementCondition>
            {
                ac1,
                ac2
            };

            Achievement achievement = new Achievement("uniqueId", "titel", "description", achievementConditions);

            int reportedProgress = 0;
            Achievement reportedAchievement = null;
            achievement.ProgressChanged += delegate(Achievement a, AchievementProgressChangedArgs args)
            {
                reportedProgress = args.ProgressCount;
                reportedAchievement = a;
            };

            // Act
            achievementConditionMock2.Raise(x => x.ProgressChanged += null, ac1,
                new AchievementConditionProgressChangedArgs(conditionProgress));

            // Assert
            Assert.AreEqual(achievement, reportedAchievement);
            Assert.AreEqual(expectedAchievementProgress, achievement.Progress);
            Assert.AreEqual(expectedAchievementProgress, reportedProgress);
        }

        [Test]
        public void ConditionProgressChanged_GivenAlreadyUnlockedAchievementCondition_DoNothing()
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");
            achievementConditionMock1.SetupGet(x => x.Progress).Returns(0);

            Mock<IAchievementCondition> achievementConditionMock2 = new Mock<IAchievementCondition>();
            achievementConditionMock2.SetupGet(x => x.UniqueId).Returns("ac2");
            achievementConditionMock2.SetupGet(x => x.Progress).Returns(150);
            achievementConditionMock2.SetupGet(x => x.Unlocked).Returns(true);

            IAchievementCondition ac1 = achievementConditionMock1.Object;
            IAchievementCondition ac2 = achievementConditionMock2.Object;
            List<IAchievementCondition> achievementConditions = new List<IAchievementCondition>
            {
                ac1,
                ac2
            };

            Achievement achievement = new Achievement("uniqueId", "titel", "description", achievementConditions);

            int reportedProgress = 0;
            Achievement reportedAchievement = null;
            achievement.ProgressChanged += delegate(Achievement a, AchievementProgressChangedArgs args)
            {
                reportedProgress = args.ProgressCount;
                reportedAchievement = a;
            };

            // Act
            achievementConditionMock2.Raise(x => x.ProgressChanged += null, ac2,
                new AchievementConditionProgressChangedArgs(150));

            // Assert
            Assert.AreEqual(0, reportedProgress);
            Assert.IsNull(reportedAchievement);
        }

        [Test]
        public void Clear_GivenAnAchievementWithSeveralConditions_ClearsTheAchievement()
        {
            // Arrange
            Mock<IAchievementCondition> achievementConditionMock1 = new Mock<IAchievementCondition>();
            achievementConditionMock1.SetupGet(x => x.UniqueId).Returns("ac1");
            achievementConditionMock1.SetupGet(x => x.Progress).Returns(0);

            Mock<IAchievementCondition> achievementConditionMock2 = new Mock<IAchievementCondition>();
            achievementConditionMock2.SetupGet(x => x.UniqueId).Returns("ac2");
            achievementConditionMock2.SetupGet(x => x.Progress).Returns(50);

            IAchievementCondition ac1 = achievementConditionMock1.Object;
            IAchievementCondition ac2 = achievementConditionMock2.Object;
            List<IAchievementCondition> achievementConditions = new List<IAchievementCondition>
            {
                ac1,
                ac2
            };

            Achievement achievement = new Achievement("uniqueId", "titel", "description", achievementConditions);

            int reportedProgress = 0;
            Achievement reportedAchievementProgressChanged = null;
            achievement.ProgressChanged += delegate(Achievement a, AchievementProgressChangedArgs args)
            {
                reportedProgress = args.ProgressCount;
                reportedAchievementProgressChanged = a;
            };
            Achievement reportedAchievementCompleted = null;
            achievement.AchievementCompleted += delegate(Achievement a) { reportedAchievementCompleted = a; };

            // Act
            achievement.Clear();
            achievementConditionMock2.Raise(x => x.ProgressChanged += null, ac2,
                new AchievementConditionProgressChangedArgs(50));
            achievementConditionMock2.Raise(x => x.ConditionCompleted += null, ac2);

            // Assert
            Assert.AreEqual(0, achievement.Conditions.Count());
            Assert.AreEqual(0, reportedProgress);
            Assert.AreEqual(null, reportedAchievementProgressChanged);
            Assert.AreEqual(null, reportedAchievementCompleted);
        }
    }
}