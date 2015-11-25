namespace sebingel.scharpchievements
{
    /// <summary>
    /// Delegate of the event that fires when the progress of an AchievementCondition is changed
    /// </summary>
    /// <param name="sender">The AchievementCondition which progress is changed</param>
    /// <param name="args">Parameters containing further information of the progress change</param>
    public delegate void AchievementConditionProgressChangedHandler(AchievementCondition sender, AchievementConditionProgressChangedArgs args);
}
