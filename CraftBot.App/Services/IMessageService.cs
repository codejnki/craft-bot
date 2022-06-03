using Discord.WebSocket;

namespace CraftBot.App.Services
{
  public interface IMessageService
  {
    public Task MessageReceivedAsync(SocketMessage rawMessage);
  }
}
