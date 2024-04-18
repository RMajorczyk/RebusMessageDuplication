using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Serialization.Json;
using RebusMessageDuplication.Events;
using RebusMessageDuplication.Handlers;

namespace RebusMessageDuplication.Config;

public static class RebusConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        services
            .AutoRegisterHandlersFromAssemblyOf<Program>()
            .AddRebusHandler<FailedMessageHandler>()
            .AddRebus(
                configure =>
                {
                    configure.Transport(
                        t =>
                        {
                            t.UseAzureServiceBus(RebusSettings.ConnectionString, RebusSettings.QueueName);
                        });

                    configure.Options(
                        options =>
                        {
                            options.SetBusName("RebusMessageDuplication");
                            options.SetNumberOfWorkers(1);
                            options.SetMaxParallelism(5);
                        });

                    configure.Events(
                        e =>
                        {
                            e.BeforeMessageHandled += LoggingEvents.LogIncomingMessage;
                            e.AfterMessageHandled += LoggingEvents.LogOutgoingMessage;
                        });

                    configure.Serialization(s => s.UseNewtonsoftJson(JsonInteroperabilityMode.PureJson));

                    configure.Options(
                        options =>
                        {
                            options.RetryStrategy(errorQueueName: RebusSettings.ErrorQueueName, maxDeliveryAttempts: 1, secondLevelRetriesEnabled: true);
                        });

                    return configure;
                }, onCreated: bus => bus.Subscribe<TestEvent>());
    }
}