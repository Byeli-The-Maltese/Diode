using System.Diagnostics.CodeAnalysis;

namespace Diode;

/// <summary>
/// Contains extensions for the result monad <see cref="Rs{T}"/> 
/// </summary>
public static class RsLinqExt
{
    public static IEnumerable<T> Successes<T>(this IEnumerable<Rs<T>> enu)
    {
        foreach (Rs<T> r in enu)
            if (r.Then(out T? v))
                yield return v;
    }
}
