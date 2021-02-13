namespace UltimateRedditBot.Discord.App.Services.Queue
{
    public class GetQueueDto
    {
        public ulong Id { get; set; }

        public string Group { get; set; }

        public ulong? ChannelId { get; set; }
    }
}
