using Rebus.Bus;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Pipeline;

namespace RebusMessageDuplication.Config;

public static class LoggingEvents
{
    public static void LogIncomingMessage(
        IBus bus,
        Dictionary<string, string> headers,
        object message,
        IncomingStepContext context,
        MessageHandledEventHandlerArgs args)
    {
        string id = headers[Headers.MessageId];
        string type = headers[Headers.Type];

        Console.WriteLine($"Received {type} (messageId: {id})");
    }

    public static void LogOutgoingMessage(
        IBus bus,
        Dictionary<string, string> headers,
        object message,
        IncomingStepContext context,
        MessageHandledEventHandlerArgs args)
    {
        string id = headers[Headers.MessageId];
        string type = headers[Headers.Type];

        Console.WriteLine($"Handled {type} (messageId: {id})");
    }
}