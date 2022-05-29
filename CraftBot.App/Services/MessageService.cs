using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CraftBot.App.Services
{
  public class MessageService
  {
    private readonly ILogger<MessageService> _logger;
    private readonly DiscordSocketClient _discordSocketClient;

    public MessageService(ILogger<MessageService> logger, DiscordSocketClient discordSocketClient)
    {
      _logger = logger;
      _discordSocketClient = discordSocketClient;

      _discordSocketClient.MessageReceived += MessageReceivedAsync;
    }

    public async Task MessageReceivedAsync(SocketMessage rawMessage)
    {
      if (rawMessage.Author.Id == _discordSocketClient.CurrentUser.Id)
      {
        return;
      }

      if (rawMessage.Content.ToLower().Contains("craft"))
      {
        _logger.LogDebug("Responding to channel: {channel}", rawMessage.Channel);
        var msg = await rawMessage.Channel.SendMessageAsync("Did someone say craft?");
        Emoji emoji = new("🍁");
        await msg.AddReactionAsync(emoji);
      }

      if (rawMessage.Content.Equals("!ping"))
      {
        var cb = new ComponentBuilder().WithButton("Click-me!", "unique-id", ButtonStyle.Primary);
        await rawMessage.Channel.SendMessageAsync("pong!", components: cb.Build());
      }
    }
  }
}
