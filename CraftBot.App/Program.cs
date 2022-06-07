using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

const string OUTPUT_TEMPLATE = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fffzzz} {Level:u3}] {Message:lj}{NewLine}{Exception}";

string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfigurationRoot configuration = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", true, true)
  .AddJsonFile($"appsettings.{environment}.json", true, true)
  .AddEnvironmentVariables()
  .Build();

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(configuration)
  .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
  .Enrich.FromLogContext()
  .WriteTo.Async(a => a.Debug(outputTemplate: OUTPUT_TEMPLATE))
  .WriteTo.Async(a =>
    a.File("logs/craft-bot.log",
      rollingInterval: RollingInterval.Day,
      encoding: System.Text.Encoding.UTF8))
  .WriteTo.Async(a => a.Console(outputTemplate: OUTPUT_TEMPLATE))
  .CreateLogger();

await Host.CreateDefaultBuilder(args)
  .UseSerilog()
  .UseServiceProviderFactory(new AutofacServiceProviderFactory())
  .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
  .RunConsoleAsync();


Log.CloseAndFlush();

static void ConfigureContainer(ContainerBuilder containerBuilder)
{
  Assembly? assembly = Assembly.GetEntryAssembly();

  if (assembly is not null)
  {
    _ = containerBuilder
      .RegisterType<DiscordSocketClient>()
        .As<DiscordSocketClient>()
        .SingleInstance();
    _ = containerBuilder
          .RegisterType<CommandService>()
          .As<CommandService>()
          .SingleInstance();
    _ = containerBuilder.RegisterAssemblyTypes(assembly)
        .As(t => t.GetInterfaces())
        .SingleInstance();
  }
  else
  {
    throw new InvalidOperationException("Unable to get entry assembly.");
  }
}
