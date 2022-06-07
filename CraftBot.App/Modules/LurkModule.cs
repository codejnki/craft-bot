using System.Text;
using CraftBot.App.Models;
using CraftBot.App.Services;
using Discord.Commands;

namespace CraftBot.App.Modules
{
  [Group("lurk")]
  public class LurkModule : ModuleBase<SocketCommandContext>
  {
    private readonly ILurkService _lurkService;

    public LurkModule(ILurkService lurkService)
    {
      _lurkService = lurkService;
    }

    [Command("list")]
    public async Task ListLurks()
    {
      StringBuilder response = new();

      foreach (KeyValuePair<string, LurkReply> entry in _lurkService.LurkList())
      {
        _ = response.AppendLine($"{entry.Key} | {entry.Value.ReplyText} | {entry.Value.ReplyEmoji}");
      }

      _ = await ReplyAsync(response.ToString());
    }

    [Command("del")]
    public async Task DeleteLurk(string lurkKey)
    {
      await Task.WhenAll(
        _lurkService.DeleteLurk(lurkKey),
        ReplyAsync($"{lurkKey} has been deleted."));
    }

    [Command("add")]
    public async Task AddLurk(string lurkKey, string lurkReply, string emoji)
    {
      await Task.WhenAll(
        _lurkService.AddLurk(lurkKey, new LurkReply(lurkReply, emoji)),
        ReplyAsync($"{lurkKey} - {lurkReply} - {emoji}"));
    }

    [Command("add")]
    public async Task AddLurk(string lurkKey, string lurkReply)
    {
      await Task.WhenAll(
        _lurkService.AddLurk(lurkKey, new LurkReply(lurkReply, string.Empty)),
        ReplyAsync($"{lurkKey} - {lurkReply}"));

    }
  }
}
