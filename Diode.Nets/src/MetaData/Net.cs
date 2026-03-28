namespace Diode.Nets;

public readonly record struct Net<T>
where T : IEquatable<T>
{
    private readonly ulong idCode;

    private Net(ulong idCode) => this.idCode = idCode;

    public readonly Net<T> ThrowIfCounterfeit()
    {
        if (idCode == default)
            throw new InvalidOperationException("The net is counterfeit and was never issued by the network");
        return this;
    }

    public static implicit operator ulong(Net<T> net) => net.idCode;

    /// <summary>
    /// Intentionally confined to this project
    /// </summary>
    internal static class Secrets
    {
        public static Net<T> FromIntegerCode(ulong idCode) => new(idCode);
    }
}