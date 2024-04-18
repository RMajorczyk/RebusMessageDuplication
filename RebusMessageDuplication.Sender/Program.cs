namespace RebusMessageDuplication.Console;

using System;
using Events;
using Rebus.Activation;
using Rebus.Config;
using RebusMessageDuplication.Config;

internal class Program
{
    static async Task Main(string[] args)
    {
        var activator = new BuiltinHandlerActivator();
        var rebusConfigurer = Configure.With(activator);

        rebusConfigurer.Transport(t => t.UseAzureServiceBus(RebusSettings.ConnectionString, RebusSettings.QueueName));

        rebusConfigurer.Start();

        var message = new TestEvent
        {
            Message = "This is a test message"
        };

        Console.WriteLine("Press any key to send a new message");
        
        Console.ReadKey();

        await activator.Bus.Publish(message);

        Console.WriteLine("\nMessage sent");
    }
}