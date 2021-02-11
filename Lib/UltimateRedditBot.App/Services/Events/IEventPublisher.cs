using System.Threading.Tasks;

namespace UltimateRedditBot.App.Services.Events
{
    public interface IEventPublisher
    {
        Task Publish<T>(T eventMessage);
    }
}