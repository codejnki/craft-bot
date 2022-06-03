using Discord;

namespace CraftBot.App.Services
{
  public interface ILoggingService
  {
    public Task OnReadyAsync();

    public Task OnLogAsync(LogMessage message);
  }
}
