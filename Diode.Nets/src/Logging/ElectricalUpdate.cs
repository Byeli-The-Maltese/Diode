namespace Diode.Nets;

public abstract record ElectricalUpdate;

public sealed record NodeUpdate(string NodeName, L6 State, object? Value) : ElectricalUpdate;

public sealed record NodeLifetime(string NodeName, LifetimeEvent Event) : ElectricalUpdate;

public sealed record SubLifetime(string SubName, LifetimeEvent Event) : ElectricalUpdate;

public sealed record LinkLifetime(string FullName, LifetimeEvent Event) : ElectricalUpdate;

public sealed record LinkUpdateStart(string FullName, L3 Magnitude, object? Value) : ElectricalUpdate;

public sealed record LinkUpdateEnd(string FullName, L3 Magnitude, object? Value, string[] NodeNames) : ElectricalUpdate;

public sealed record NetworkVoltageFlush(int RecursionDepth) : ElectricalUpdate;

public sealed record NetworkStimulus(CallSite CallSite, int RecursionDepth) : ElectricalUpdate;

public enum LifetimeEvent
{
    Construct,
    Dispose
}
