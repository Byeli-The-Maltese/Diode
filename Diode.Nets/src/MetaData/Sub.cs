namespace Diode.Nets;

public readonly record struct Sub
{
    private readonly ulong idCode;

    internal Sub(ulong idCode) => this.idCode = idCode;

    public Sub ThrowIfCounterfeit()
    {
        if (idCode == default)
            throw new InvalidOperationException("The sub is counterfeit and was never issued by the network");
        return this;
    }

    public static implicit operator ulong(Sub sub) => sub.idCode;

    /// <summary>
    /// Intentionally confined to this project
    /// </summary>
    internal static class Secrets
    {
        public static Sub FromIntegerCode(ulong idCode) => new(idCode);
    }
}
