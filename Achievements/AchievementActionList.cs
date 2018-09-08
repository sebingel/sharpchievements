using System.Collections.Generic;

namespace sebingel.sharpchievements
{
    public sealed class AchievementActionList
    {
        #region - Konstruktoren -

        public AchievementActionList() => this.RegisteredAchievementActions = new List<AchievementAction>();

        #endregion

        #region - Properties oeffentlich -

        public IReadOnlyCollection<AchievementAction> RegisteredAchievementActions { get; }

        #endregion
    }
}