using System.Collections.Immutable;

namespace Diode.Nets;

public partial class Network
{
    // // // fields
    private readonly Mutex stimtex = new();
    private readonly Stack<int> stimulusCounts = [];
    private ImmutableList<ElectricalUpdate> stimHistory = [];
    private Options options = default;

    // // // methods

    private ImmutableList<ElectricalUpdate> Stimulate<T>(Net<T> net, Voltage<T> voltage, CallSite callSite)
    where T : IEquatable<T>
    {
        ImmutableList<ElectricalUpdate> retList;
        stimtex.WaitOne();
        stimulusCounts.Push(stimHistory.Count);
        LogStimuli(callSite, stimulusCounts.Count - 1);
        try
        {
            nodes.TryGet(net).Match(node => node.TakeInput(links.StimuliFakeId, voltage));
            FlushVoltages();
        }
        finally
        {
            int count = stimulusCounts.Pop();
            int addedThisTime = stimHistory.Count - count;
            retList = stimHistory.GetRange(count, addedThisTime);
            stimHistory = stimHistory.GetRange(0, count);
            stimtex.ReleaseMutex(); 
        }
        return retList;
    }

    private void SetOptions(Options options)
    {
        stimtex.WaitOne();
        try { this.options = options; }
        finally { stimtex.ReleaseMutex(); }
    }

    private void LogNodeChange(NodeCache.Node node, object? value)
    {
        if (options.LogNodeStateChange)
            stimHistory = stimHistory.Add(
                new NodeUpdate(node.GetFullName(), node.State, value)
            );
    }

    private void LogNodeCreation(NodeCache.Node node)
    {
        if (options.LogNodeLifetimes)
            stimHistory = stimHistory.Add(
                new NodeLifetime(node.GetFullName(), LifetimeEvent.Construct)
            );
    }

    private void LogNodeDestruction(NodeCache.Node node)
    {
        if (options.LogNodeLifetimes)
            stimHistory = stimHistory.Add(new NodeLifetime(node.GetFullName(), LifetimeEvent.Dispose));
    }

    private void LogSubCreation(SubCache.Subcircuit subc)
    {
        if (options.LogSubcircuitLifetimes)
            stimHistory = stimHistory.Add(new SubLifetime(subc.GetFullName(), LifetimeEvent.Construct));
    }

    private void LogSubDestruction(SubCache.Subcircuit subc)
    {
        if (options.LogSubcircuitLifetimes)
            stimHistory = stimHistory.Add(new SubLifetime(subc.GetFullName(), LifetimeEvent.Dispose));
    }

    private void LogLinkCreation(LinkCache.Link link)
    {
        if (options.LogLinkLifetimes)
            stimHistory = stimHistory.Add(new LinkLifetime(link.GetFullName(), LifetimeEvent.Construct));
    }

    private void LogLinkDestruction(LinkCache.Link link)
    {
        if (options.LogLinkLifetimes)
            stimHistory = stimHistory.Add(new LinkLifetime(link.GetFullName(), LifetimeEvent.Dispose));
    }

    private void LogLinkDriveStart(LinkCache.Link link, L3 magnitude, object? value )
    {
        if (options.LogLinkDrives)
            stimHistory = stimHistory.Add(new LinkUpdateStart(link.GetFullName(), magnitude, value));
    }

    private void LogLinkDriveFinish(LinkCache.Link link, L3 magnitude, object? value)
    {
        if (options.LogLinkDrives)
            stimHistory = stimHistory.Add(new LinkUpdateEnd(link.GetFullName(), magnitude, value, link.GetTargetNodeNames()));
    }

    private void LogVoltageFlush(int loops)
    {
        if (options.LogVoltageFlushes)
            stimHistory = stimHistory.Add(new NetworkVoltageFlush(loops));
    }

    private void LogStimuli(CallSite callSite, int recursionCount)
    {
        if (options.LogStimuli)
            stimHistory = stimHistory.Add(new NetworkStimulus(callSite, recursionCount));
    }

}