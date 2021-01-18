namespace UltimateRedditBot.Domain.Queue
{
    public interface IAddToQueueOptions
    {
        string Group { get; set; }

        ulong ClientId { get; set; }
    }
}
