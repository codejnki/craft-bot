using CraftBot.App.Models;

namespace CraftBot.App.Services
{
  public class LurkService : ILurkService
  {

    private readonly Dictionary<string, LurkReply> _lurkDictionary = new();

    public LurkService()
    {
      _lurkDictionary["craft"] = new LurkReply("Did somebody say craft?", "🍁");
      _lurkDictionary["foo"] = new LurkReply("Foo you buddy", "😒");
    }

    public Task AddLurk(string lurkKey, LurkReply lurkReply)
    {
      _lurkDictionary.Add(lurkKey, lurkReply);
      return Task.CompletedTask;
    }

    public Task DeleteLurk(string lurkKey)
    {
      _lurkDictionary.Remove(lurkKey);
      return Task.CompletedTask;
    }

    public Task<LurkReply> LurkChannel(string messageContent)
    {
      foreach (KeyValuePair<string, LurkReply> entry in _lurkDictionary)
      {
        if (messageContent.ToLowerInvariant().Contains(entry.Key))
        {
          return Task.FromResult(entry.Value);
        }
      }
      return Task.FromResult<LurkReply>(null);
    }

    public Dictionary<string, LurkReply> LurkList()
    {
      return _lurkDictionary;
    }
  }
}
