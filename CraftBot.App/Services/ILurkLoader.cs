using CraftBot.App.Models;

namespace CraftBot.App.Services
{
  public interface ILurkLoader
  {
    Dictionary<string, LurkReply> LoadLurkList();

    Task SaveLurkList(Dictionary<string, LurkReply> lurkList);
  }
}
