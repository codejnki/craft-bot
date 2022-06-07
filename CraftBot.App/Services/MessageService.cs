using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CraftBot.App.Services
{
  public class MessageService : IMessageService
  {
    private readonly ILogger<MessageService> _logger;
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly ILurkService _lurkService;

    public MessageService(
      ILogger<MessageService> logger,
      DiscordSocketClient discordSocketClient,
      ILurkService lurkService)
    {
      _logger = logger;
      _discordSocketClient = discordSocketClient;
      _lurkService = lurkService;
    }

    public async Task MessageReceivedAsync(SocketMessage rawMessage)
    {
      if (rawMessage.Author.Id == _discordSocketClient.CurrentUser.Id)
      {
        return;
      }

      var lurkReply = await _lurkService.LurkChannel(rawMessage.Content);

      if (lurkReply is not null)
      {
        var tasks = new List<Task>();
        _logger.LogDebug("Responding to channel: {channel}", rawMessage.Channel);

        tasks.Add(rawMessage.Channel.TriggerTypingAsync());

        var msg = rawMessage.Channel.SendMessageAsync(lurkReply.ReplyText, messageReference: rawMessage.Reference);
        tasks.Add(msg);
        
        if (lurkReply.ReplyEmoji is not null) 
        {
          tasks.Add(msg.Result.AddReactionAsync(lurkReply.ReplyEmoji));
        }

        await Task.WhenAll(tasks);
      }
    }
  }
}
