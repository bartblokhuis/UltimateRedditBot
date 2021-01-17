using System.Threading.Tasks;

namespace UltimateRedditBot.App.Services.Events
{
    public interface IConsumer<T>
    {
        Task HandleEvent(T eventMessage);
    }
}
