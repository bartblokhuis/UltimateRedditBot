namespace UltimateRedditBot.Domain.Dtos.Common
{
    public interface IBaseDto<TKEy>
    {
    }

    public interface IBaseDto : IBaseDto<int>
    {
    }
}