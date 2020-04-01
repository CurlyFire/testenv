using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace testenv
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder<Startup>(args)
            .UseStartup<Startup>()
            .UseUrls("http://*:5000")
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.Sources.Clear();
                configurationBuilder.AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.user.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
            })
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders()
                .AddConsole()
                .AddDebug();
            })
            .Build()
            .Run();
        }
    }
}