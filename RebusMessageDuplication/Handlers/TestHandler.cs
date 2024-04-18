namespace RebusMessageDuplication.Handlers;

using Rebus.Handlers;
using RebusMessageDuplication.Events;

public class TestHandler : IHandleMessages<TestEvent>
{
    public async Task Handle(TestEvent message)
    {
        await Task.Delay(TimeSpan.FromSeconds(45));

        throw new Exception("Timeout");
    }
}