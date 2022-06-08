using CraftBot.App.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CraftBot.App.Services
{
  public class LurkLoader : ILurkLoader
  {
    private ILogger<LurkLoader> _logger;
    private readonly CraftBotSettings _craftBotSettings;
    private readonly string _folderPath;
    private readonly string _filePath;

    public LurkLoader(
      ILogger<LurkLoader> logger,
      CraftBotSettings craftBotSettings) {
      
      _logger = logger;
      _craftBotSettings = craftBotSettings;

      _folderPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), _craftBotSettings.CraftBotFolder);
      _filePath = Path.Join(_folderPath, _craftBotSettings.LurksFile);
    }

    public Dictionary<string, LurkReply> LoadLurkList()
    {
      if (Directory.Exists(_folderPath) == false)
      {
        CreateCraftbotFolder();
      }

      if (File.Exists(_filePath) == false)
      {
        CreateDefaultLurkList();
      }

      var lurkJson = File.ReadAllText(_filePath);
      var lurkDictionary = JsonConvert.DeserializeObject<Dictionary<string, LurkReply>>(lurkJson) ?? new Dictionary<string, LurkReply>();
      _logger.LogInformation("Loaded {lurkCount} from {filePath}.", lurkDictionary.Count, _filePath);
      return lurkDictionary;
    }

    private void CreateDefaultLurkList()
    {
      _logger.LogDebug("Creating default lurk file in {filePath}", _filePath);
      Task.Run(async () => await SaveLurkList(GetDefaultLurks())).Wait();
    }

    private void CreateCraftbotFolder()
    {
      _logger.LogDebug("Creating craftbot folder in {folderPath}", _folderPath);
      Directory.CreateDirectory(_folderPath);
    }

    public async Task SaveLurkList(Dictionary<string, LurkReply> lurkList)
    {
      using var fileStream = File.CreateText(_filePath);
      _logger.LogInformation("Saving {lurkCount} lurk(s) to {filePath}.", lurkList.Count, _filePath);
      var serializer = new JsonSerializer();
      serializer.Serialize(fileStream, lurkList);
      await fileStream.DisposeAsync();
    }

    private static Dictionary<string, LurkReply> GetDefaultLurks() {
      var lurkDictionary = new Dictionary<string, LurkReply>
      {
        ["craft"] = new LurkReply("Did somebody say craft?", "🍁"),
        ["foo"] = new LurkReply("Foo you buddy", "😒")
      };

      return lurkDictionary;
    }
  }
}
