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

public sealed record NodeLifetime(string NodeName, LifeCycle Event) : ElectricalUpdate
{
    public override string ToString() => Event switch
    {
        LifeCycle.Create => $"NEW NET {NodeName}",
        LifeCycle.Destroy => $"END NET {NodeName}",
        _ => throw new("Invalid state")
    };
}

public sealed record SubLifetime(string SubName, LifeCycle Event) : ElectricalUpdate
{
    public override string ToString() => Event switch
    {
        LifeCycle.Create => $"NEW SUB {SubName}",
        LifeCycle.Destroy => $"END SUB {SubName}",
        _ => throw new("Invalid state")
    };
}

public sealed record LinkLifetime(string FullName, LifeCycle Event) : ElectricalUpdate
{
    public override string ToString() => Event switch
    {
        LifeCycle.Create => $"NEW LINK {FullName}",
        LifeCycle.Destroy => $"END LINK {FullName}",
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
