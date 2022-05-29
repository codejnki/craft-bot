using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CraftBot.App.Services
{
  public class LoggingService
  {
    private readonly ILogger _logger;
    private readonly DiscordSocketClient _discordSocketClient;

    public LoggingService(ILogger<LoggingService> logger, DiscordSocketClient discordSocketClient)
    {
      _logger = logger;
      _discordSocketClient = discordSocketClient;

      _discordSocketClient.Ready += OnReadyAsync;
      _discordSocketClient.Log += OnLogAsync;
    }

    public Task OnReadyAsync()
    {
      // Regex to remove some non printable characters
      _logger.LogInformation("Connected as -> [{CurrentUser}]", Regex.Replace(_discordSocketClient.CurrentUser.ToString(), @"\p{C}+", string.Empty));
      return Task.CompletedTask;
    }

    public Task OnLogAsync(LogMessage message)
    {
      string logText = $"{message.Source}: {message.Exception?.ToString() ?? message.Message}";

      switch (message.Severity)
      {
        case LogSeverity.Critical:
          {
            _logger.LogCritical("{logText}", logText);
            break;
          }
        case LogSeverity.Warning:
          {
            _logger.LogWarning("{logText}", logText);
            break;
          }
        case LogSeverity.Info:
          {
            _logger.LogInformation("{logText}", logText);
            break;
          }
        case LogSeverity.Verbose:
          {
            _logger.LogInformation("{logText}", logText);
            break;
          }
        case LogSeverity.Debug:
          {
            _logger.LogDebug("{logText}", logText);
            break;
          }
        case LogSeverity.Error:
          {
            _logger.LogError("{logText}", logText);
            break;
          }

        default:
          break;
      }
      return Task.CompletedTask;
    }
  }
}
