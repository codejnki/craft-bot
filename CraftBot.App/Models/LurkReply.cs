using Discord;
using Newtonsoft.Json;

namespace CraftBot.App.Models
{
  public class LurkReply
  {
    public string ReplyText { get; set; }

    // TODO: This is hacky, figure out a cleaner way to handle this
    [JsonIgnore]
    public Emoji ReplyEmoji => new(ReplyEmojiText);

    public string ReplyEmojiText { get; set; }

    public LurkReply(string replyText, string replyEmojiText)
    {
      ReplyText = replyText;
      ReplyEmojiText = replyEmojiText;
    }
  }
}
