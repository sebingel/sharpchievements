namespace sebingel.scharpchievements
{
    /// <summary>
    /// Delegate of the event that fires when the progress of an Achievement is changed
    /// </summary>
    /// <param name="sender">The Achievement which progress is changed</param>
    /// <param name="args">Parameters containing further information of the progress change</param>
    public delegate void AchievementProgressChangedHandler(Achievement sender, AchievementProgressChangedArgs args);
}
