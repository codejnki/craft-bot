using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraftBot.App.Services
{
  public class CommandHandlingService : ICommandHandlingService
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
      _serviceProvider = serviceProvider; // I hate this...we shouldn't be injecting the service provider
    }

    public async Task InstallCommandsAsync()
    {
      _commandService.CommandExecuted += CommandExecutedAsync;
      _ = await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
    }

    public async Task MessageReceivedAsync(SocketMessage rawMessage)
    {
      // Ignore system messages, or messages from other bots
      if (rawMessage is not SocketUserMessage message || message.Source != MessageSource.User)
      {
        return;
      }

      int argPos = 0;
      if (!message.HasCharPrefix('~', ref argPos))
      {
        return;
      }

      var typing = rawMessage.Channel.TriggerTypingAsync();
      var context = new SocketCommandContext(_discordSocketClient, message);
      var command = _commandService.ExecuteAsync(context, argPos, _serviceProvider);

      await Task.WhenAll(typing, command);
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

      _ = await context.Channel.SendMessageAsync($"error: {result}");
    }

  }
}
