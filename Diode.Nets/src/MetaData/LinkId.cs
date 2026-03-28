namespace Diode.Nets;

public readonly record struct LinkId
{
    private readonly ulong idCode;

    private LinkId(ulong idCode) => this.idCode = idCode;

    public static implicit operator ulong(LinkId self) => self.idCode;

    internal static class Secrets
    {
        internal static LinkId FromIntegerCode(ulong idCode) => new(idCode);
    }
}
