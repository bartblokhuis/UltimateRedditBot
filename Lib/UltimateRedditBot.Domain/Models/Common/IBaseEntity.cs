namespace UltimateRedditBot.Domain.Models.Common
{
    public interface IBaseEntity<TKey>
    {
    }

    public interface IBaseEntity : IBaseEntity<int>
    {
    }
}