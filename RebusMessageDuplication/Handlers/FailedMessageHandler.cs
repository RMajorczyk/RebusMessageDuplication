using System.Globalization;
using System.Text;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Messages;
using Rebus.Retry;
using Rebus.Retry.Simple;
using RebusMessageDuplication.Config;

namespace RebusMessageDuplication.Handlers;

public class FailedMessageHandler : IHandleMessages<IFailed<object>>
{
    private readonly IBus _bus;

    public FailedMessageHandler(
        IBus bus)
    {
        _bus = bus;
    }

    public Task Handle(IFailed<object> message)
    {
        var retriesCount = Convert.ToInt32(message.Headers.GetValueOrDefault(Headers.DeferCount), CultureInfo.InvariantCulture);

        List<ExceptionInfo> exceptionInfos = message.Exceptions?.ToList() ?? new List<ExceptionInfo>();
        var exceptionText = new StringBuilder();

        if (exceptionInfos.Count > 1)
        {
            exceptionText.AppendLine($"{exceptionInfos.Count} unhandled exceptions");
        }

        foreach (ExceptionInfo exceptionInfo in exceptionInfos)
        {
            exceptionText.AppendLine(exceptionInfo.GetFullErrorDescription());
            exceptionText.AppendLine();
        }

        Dictionary<string, string> headers = message.Headers;

        if (headers.ContainsKey(Headers.ErrorDetails))
        {
            headers[Headers.ErrorDetails] += $"\n{exceptionText}";
        }
        else
        {
            headers.Add(Headers.ErrorDetails, exceptionText.ToString());
        }

        exceptionText.Clear();

        if (!headers.ContainsKey(Headers.ErrorDetails))
        {
            headers[Headers.SourceQueue] = RebusSettings.ErrorQueueName;
        }

        if (retriesCount >= 8)
        {
            return _bus.Advanced.TransportMessage.Forward(RebusSettings.ErrorQueueName, message.Headers);
        }

        if ((retriesCount + 1) % 3 == 0)
        {
            return _bus.Advanced.TransportMessage.Defer(TimeSpan.FromSeconds(15), headers);
        }

        return _bus.Advanced.TransportMessage.Defer(TimeSpan.Zero, headers);
    }

}