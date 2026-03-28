namespace Diode.Nets;

public abstract record VoltagePush(ulong OriginRaw, LinkId PushThroush);

public sealed record VoltagePush<T>(Net<T> Origin, LinkId PushThrough, Voltage<T> Voltage) : VoltagePush(Origin, PushThrough)
where T : IEquatable<T>;