using System.Diagnostics.CodeAnalysis;

namespace Diode.Nets;

public readonly record struct Voltage<T> where T : IEquatable<T>
{
    private readonly T? Value;

    private Voltage(T Value, bool Strong)
        => (Magnitude, this.Value) = (Strong ? L3.S : L3.W, Value);

    public L3 Magnitude { get; }

    public Rs<T> Resolve() => Magnitude is L3.Z
        ? new Er("Voltage is off")
        : Value!;

    internal T? RawValue() => Value;

    public bool IsWeak([NotNullWhen(true)] out T? value)
    {
        if (Magnitude is L3.W)
        {
            value = Value!;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    public bool IsStrong([NotNullWhen(true)] out T? value)
    {
        if (Magnitude is L3.S)
        {
            value = Value!;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    public static Voltage<T> Strong(T Value) => new(Value, true);

    public static Voltage<T> Weak(T Value) => new(Value, false);
}