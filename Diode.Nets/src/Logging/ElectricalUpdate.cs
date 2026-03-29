namespace Diode.Nets;

public abstract record ElectricalUpdate;

public sealed record NodeUpdate(string NodeName, L6 State, object? Value) : ElectricalUpdate
{
    public override string ToString() => State switch
    {
        L6.S => $"{NodeName} = {Value}",
        L6.W => $"{NodeName} ~ {Value}",
        L6.N => $"{NodeName} is Noise",
        L6.U => $"{NodeName} is Uninitialized",
        L6.Z => $"{NodeName} is Hi Z",
        L6.X => $"{NodeName} is HAZARD",
        _ => throw new("Invalid state")
    };
}

public sealed record NodeLifetime(string NodeName, LifetimeEvent Event) : ElectricalUpdate
{
    public override string ToString() => Event switch
    {
        LifetimeEvent.Construct => $"NEW NET {NodeName}",
        LifetimeEvent.Dispose => $"END NET {NodeName}",
        _ => throw new("Invalid state")
    };
}

public sealed record SubLifetime(string SubName, LifetimeEvent Event) : ElectricalUpdate
{
    public override string ToString() => Event switch
    {
        LifetimeEvent.Construct => $"NEW SUB {SubName}",
        LifetimeEvent.Dispose => $"END SUB {SubName}",
        _ => throw new("Invalid state")
    };
}

public sealed record LinkLifetime(string FullName, LifetimeEvent Event) : ElectricalUpdate
{
    public override string ToString() => Event switch
    {
        LifetimeEvent.Construct => $"NEW LINK {FullName}",
        LifetimeEvent.Dispose => $"END LINK {FullName}",
        _ => throw new("Invalid state")
    };
}

public sealed record LinkUpdateStart(string FullName, L3 Magnitude, object? Value) : ElectricalUpdate
{
    public override string ToString() => Magnitude switch
    {
        L3.S => $"{FullName} << Strong{{Value}}",
        L3.W => $"{FullName} <<   Weak{{Value}}",
        L3.Z => $"{FullName} << OFF",
        _ => throw new ("Invalid state")
    };
}

public sealed record LinkUpdateEnd(string FullName, L3 Magnitude, object? Value, string[] NodeNames) : ElectricalUpdate
{
    public override string ToString() => Magnitude switch
    {
        L3.S => $"{FullName} >> Strong{{Value}}",
        L3.W => $"{FullName} >>   Weak{{Value}}",
        L3.Z => $"{FullName} >> OFF",
        _ => throw new ("Invalid state")
    };
}

public sealed record NetworkVoltageFlush(int RecursionDepth) : ElectricalUpdate
{
    public override string ToString() => $"FLUSH #{RecursionDepth}";
}

public sealed record NetworkStimulus(CallSite CallSite, int RecursionDepth) : ElectricalUpdate
{
    public override string ToString() => $"STIM {CallSite.CallerName} #{RecursionDepth}";
}

public enum LifetimeEvent
{
    Construct,
    Dispose
}
