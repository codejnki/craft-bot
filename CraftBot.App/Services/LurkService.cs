using CraftBot.App.Models;

namespace CraftBot.App.Services
{
  public class LurkService : ILurkService
  {

    private readonly Dictionary<string, LurkReply> _lurkDictionary = new();
    private readonly ILurkLoader _lurkLoader;

    public LurkService(ILurkLoader lurkLoader)
    {

      _lurkLoader = lurkLoader;

      _lurkDictionary = _lurkLoader.LoadLurkList();
    }

    public async Task AddLurk(string lurkKey, LurkReply lurkReply)
    {
      _lurkDictionary.Add(lurkKey, lurkReply);
      await _lurkLoader.SaveLurkList(_lurkDictionary);
    }

    public async Task DeleteLurk(string lurkKey)
    {
      _lurkDictionary.Remove(lurkKey);
      await _lurkLoader.SaveLurkList(_lurkDictionary);
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
