using Discord;
using Discord.Commands;

namespace CraftBot.App.Services
{
  public class PublicModules : ModuleBase<SocketCommandContext>
  {
    [Command("ping")]
    [Alias("pong", "hello")]
    public Task PingAsync() => ReplyAsync("pong!");

    [Command("userinfo")]
    public async Task UserINfoAsync(IUser? user = null)
    {
      user ??= Context.User;
      await ReplyAsync(user.ToString());
    }

    [Command("list")]
    public async Task ListAsync(params string[] arguments)
    {
      await ReplyAsync("You listed: " + string.Join("; ", arguments));
    }
  }
}
