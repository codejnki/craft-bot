using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CraftBot.App.Services
{
  public class MessageService : IMessageService
  {
    private readonly ILogger<MessageService> _logger;
    private readonly DiscordSocketClient _discordSocketClient;

    public MessageService(ILogger<MessageService> logger, DiscordSocketClient discordSocketClient)
    {
      _logger = logger;
      _discordSocketClient = discordSocketClient;
    }

    public async Task MessageReceivedAsync(SocketMessage rawMessage)
    {
      if (rawMessage.Author.Id == _discordSocketClient.CurrentUser.Id)
      {
        return;
      }

      if (rawMessage.Content.ToLowerInvariant().Contains("craft"))
      {
        _logger.LogDebug("Responding to channel: {channel}", rawMessage.Channel);
        
        var typing =  rawMessage.Channel.TriggerTypingAsync();
        var msg = rawMessage.Channel.SendMessageAsync("Did someone say craft?", messageReference: rawMessage.Reference);
        
        Emoji emoji = new("🍁");

        await typing;
        await Task.WhenAll(msg, msg.Result.AddReactionAsync(emoji));
      }
    }
  }
}
