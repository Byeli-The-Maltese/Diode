namespace Diode;

/// <summary>
/// An error that contains a message
/// </summary>
/// <param name="Msg">The message. Please do not put newlines in it</param>
public record Er(string Msg) : IEr
{
    public override string ToString() => Msg;
}