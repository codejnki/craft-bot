using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraftBot.App.Services
{
  public interface ICommandHandlingService
  {
    Task MessageReceivedAsync(SocketMessage rawMessage);
    Task InstallCommandsAsync();
    Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result);
  }
}
