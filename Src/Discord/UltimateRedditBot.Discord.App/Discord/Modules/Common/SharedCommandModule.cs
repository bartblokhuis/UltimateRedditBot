namespace UltimateRedditBot.Discord.App.Discord.Modules.Common
{
    public class SharedCommandModule : UltimateCommandModule
    {
        #region Utils

        protected bool IsForGuild()
        {
            return Context.Guild != null;
        }

        #endregion
    }
}
