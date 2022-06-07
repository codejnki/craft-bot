using Discord;

namespace CraftBot.App.Models
{
  public class LurkReply
  {
    public string ReplyText { get; set; }

    public Emoji ReplyEmoji { get; set; }

    public LurkReply(string replyText, string replyEmoji)
    {
      ReplyText = replyText;

      if (!string.IsNullOrEmpty(replyEmoji)) {
        ReplyEmoji = new Emoji(replyEmoji);
      }
    }
  }
}
