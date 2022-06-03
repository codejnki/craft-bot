using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraftBot.App.Services
{
  public class CommandHandlingService
  {
    private readonly CommandService _commandService;
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlingService(
      CommandService commandService,
      DiscordSocketClient discordSocketClient,
      IServiceProvider serviceProvider)
    {
      _commandService = commandService;
      _discordSocketClient = discordSocketClient;
      _serviceProvider = serviceProvider;
      _commandService.CommandExecuted += CommandExecutedAsync;
      _discordSocketClient.MessageReceived += MessageReceivedAsync;
    }

    public async Task MessageReceivedAsync(SocketMessage rawMessage)
    {
      // Ignore system messages, or messages from other bots
      if (rawMessage is not SocketUserMessage message || message.Source != MessageSource.User)
      {
        return;
      }

      var argPos = 0;
      if (!message.HasCharPrefix('!', ref argPos))
      {
        return;
      }

      var context = new SocketCommandContext(_discordSocketClient, message);
      await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
    }

    public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
      if (command.IsSpecified == false)
      {
        return;
      }

      if (result.IsSuccess)
      {
        return;
      }

      await context.Channel.SendMessageAsync($"error: {result}");
    }

  }
}
