namespace Diode.Nets;

internal class SpiceSpace(string Prefix = "")
{
    internal string Prefix { get; } = Prefix;

    internal Dictionary<SpiceName, ulong> Nets { get; } = [];
    internal Dictionary<SpiceName, Sub> Subs { get; } = [];
    internal Dictionary<SpiceName, LinkId> Links { get; } = [];
}
