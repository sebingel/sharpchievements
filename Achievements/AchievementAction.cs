using System;

namespace sebingel.sharpchievements
{
    public sealed class AchievementAction
    {
        #region - Konstruktoren -

        public AchievementAction(int achievementActionId) => this.AchievementActionId = achievementActionId;

        #endregion

        #region - Methoden oeffentlich -

        public void ExecuteAction()
        {
            this.ActionExecuted?.Invoke();
        }

        #endregion

        #region - Properties oeffentlich -

        public int AchievementActionId { get; }

        #endregion

        public event Action ActionExecuted;
    }
}