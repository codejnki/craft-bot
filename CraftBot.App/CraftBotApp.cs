using CraftBot.App.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CraftBot.App
{
  internal sealed class CraftBotApp : IHostedService
  {

    private readonly ILogger _logger;

    private readonly IHostApplicationLifetime _applicationLifetime;

    private readonly DiscordSocketClient _discordClient;
    private readonly IMessageService _messageService;
    private readonly ILoggingService _loggingService;
    private int _exitCode;

    public CraftBotApp(
      ILogger<CraftBotApp> logger,
      IHostApplicationLifetime applicationLifetime,
      DiscordSocketClient discordClient,
      IMessageService messageService,
      ILoggingService loggingService)
    {
      _logger = logger;
      _applicationLifetime = applicationLifetime;
      _discordClient = discordClient;
      _messageService = messageService;
      _loggingService = loggingService;
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
      _logger.LogDebug("Starting with arguments: {}", string.Join(" ", Environment.GetCommandLineArgs()));
      _ = _applicationLifetime.ApplicationStarted.Register(() =>
      {
        _ = Task.Run(async () =>
        {
          try
          {
            string? token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

            if (string.IsNullOrEmpty(token))
            {
              throw new InvalidOperationException("No DISCORD_TOKEN found.");
            }

            await _discordClient.LoginAsync(TokenType.Bot, token);

            await _discordClient.StartAsync();
            
            

            _discordClient.Ready += _loggingService.OnReadyAsync;
            _discordClient.Log += _loggingService.OnLogAsync;
            _discordClient.MessageReceived += _messageService.MessageReceivedAsync;

            await Task.Delay(Timeout.Infinite);
          }
          catch (Exception ex)
          {
            _logger.LogCritical(ex, "Unhandled exception!");
            _exitCode = 1;
          }
          finally
          {
            // Stop the app
            _applicationLifetime.StopApplication();
          }
        });
      });

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogDebug("Exiting with return code: {exitCode}", _exitCode);
      Environment.ExitCode = _exitCode;
      return Task.CompletedTask;
    }
  }
}
