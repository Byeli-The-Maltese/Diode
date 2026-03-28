namespace Diode;

/// <summary>
/// The most general interface for an error. Defines a message
/// </summary>
public interface IEr
{
    /// <summary>
    /// The message in the error. Should not contain newlines.
    /// </summary>
    public string Msg { get; }
}