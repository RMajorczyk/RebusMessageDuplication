namespace RebusMessageDuplication;

using Config;
using Microsoft.Extensions.Hosting;

internal class Program
{
    static void Main(string[] args)
    {
        IHost hostBuilder = CreateHostBuilder(args).Build();
        hostBuilder.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(
                (hostContext, services) =>
                {
                    RebusConfiguration.Configure(services);
                })
            .UseWindowsService();
    }
}