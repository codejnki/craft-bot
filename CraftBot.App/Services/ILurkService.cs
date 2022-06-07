using CraftBot.App.Models;

namespace CraftBot.App.Services
{
  public interface ILurkService
  {
    Task<LurkReply> LurkChannel(string messageContent);

    Dictionary<string, LurkReply> LurkList();

    Task DeleteLurk(string lurkKey);

    Task AddLurk(string lurkKey, LurkReply lurkReply);
  }
}
