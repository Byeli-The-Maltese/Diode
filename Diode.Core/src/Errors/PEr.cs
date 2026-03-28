using System.Collections.Immutable;
using System.Text;

namespace Diode;

/// <summary>
/// An error that is an aggregate of many errors.
/// </summary>
public record PEr : IEr
{
    /// <summary>
    /// The top-level message that describes all of the submessages
    /// </summary>
    public required string RootMessage { get; init; }

    /// <summary>
    /// An immutable list that contains nested errors 
    /// </summary>
    public required ImmutableList<IEr> SubErrors { get; init; }

    public string Msg
    {
        get
        {
            StringBuilder sb = new();
            sb.AppendLine(RootMessage);
            foreach (var inner in SubErrors.SkipLast(1))
            {
                sb.Append("    ");
                sb.AppendLine(inner.Msg.ReplaceLineEndings(Environment.NewLine + "    "));
            }
            foreach (var inner in SubErrors.TakeLast(1))
            {
                sb.Append("    ");
                sb.Append(inner.Msg.ReplaceLineEndings(Environment.NewLine + "    "));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Creates another parallel error that contains the next sub error
    /// </summary>
    /// <param name="next">Sub error to add to this parallel error and create a new error</param>
    /// <returns>A new parallel error that includes the next error</returns>
    public PEr AbsorbNext(IEr next) => new()
    {
        RootMessage = RootMessage,
        SubErrors = SubErrors.Add(next)
    };

    /// <summary>
    /// Creates another parallel error that contains the next sub error
    /// </summary>
    /// <param name="nextMsg">Message to create a sub error</param>
    /// <returns>A new parallel error that includes the next error</returns>
    public PEr AbsorbNext(string nextMsg) => AbsorbNext(new Er(nextMsg));


    public override string ToString() => Msg;
}