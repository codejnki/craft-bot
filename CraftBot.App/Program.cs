using CraftBot.App.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CraftBot.App
{
  internal class Program
  {

    private static void Main(string[] args)
    {
      string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{environment}.json", true, true)
        .AddEnvironmentVariables()
        .Build();

      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .WriteTo.File("logs/craft-bot.log", rollingInterval: RollingInterval.Day, encoding: System.Text.Encoding.UTF8)
        .WriteTo.Console()
        .CreateLogger();

      new Program()
          .MainAsync()
          .GetAwaiter()
          .GetResult();
    }

    public Program()
    {

    }

    private async Task MainAsync()
    {
      var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

      if (string.IsNullOrEmpty(token))
      {
        throw new ArgumentNullException("No DISCORD_TOKEN value found");
      }

      using ServiceProvider services = ConfigureServices();
      var client = services.GetRequiredService<DiscordSocketClient>();

      services.GetRequiredService<LoggingService>();

      await client.LoginAsync(TokenType.Bot, token);
      await client.StartAsync();

      services.GetRequiredService<MessageService>();

      await Task.Delay(Timeout.Infinite);
    }

    private static ServiceProvider ConfigureServices()
    {
      IServiceCollection services = new ServiceCollection()
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton<LoggingService>()
        .AddSingleton<MessageService>()
        .AddLogging(configure =>
        {
          _ = configure.AddSerilog();
        });

      return services.BuildServiceProvider();
    }
  }
}
