using System.Diagnostics.CodeAnalysis;

namespace Diode;

/// <summary>
/// Contains extensions for the result monad <see cref="Rs{T}"/> 
/// </summary>
public static class RsExt
{
    /// <summary>
    /// Converts any object into a successful result 
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="t">Value to be wrapped</param>
    /// <returns>The successful result type</returns>
    public static Rs<T> ToRs<T>(this T t) => new(true, t, null);


    /// <summary>
    /// Converts an error into an failed result
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="er">Error to be wrapped</param>
    /// <returns>The failured result type</returns>
    public static Rs<T> ToFailRs<T>(this IEr er) => new(false, default, er);

    /// <summary>
    /// Performs an action with the success type if the result is a success
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="res">result to match</param>
    /// <param name="action">action that operates on the success value</param>
    public static void Match<T>(this Rs<T> res, Action<T> action)
    {
        if (res.IsOk)
            action(res.Value!);
    }

    /// <summary>
    /// Performs a side effect with the success type if the result is a success
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="res">result to match</param>
    /// <param name="action">action that operates on the success value</param>
    /// <returns>The result</returns>
    public static Rs<T> Apply<T>(this Rs<T> res, Action<T> action)
    {
        if (res.IsOk)
            action(res.Value!);
        return res;
    }

    /// <summary>
    /// Transforms a result type into another result type, using
    /// the specified function to convert the successful value
    /// into another successful value. If the converted result is
    /// a failure, then that failure will be passed on to the to
    /// returned result.
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <typeparam name="U">The success type of the transformed result</typeparam>
    /// <param name="res">result to map</param>
    /// <param name="func">function that operates on the success value</param>
    /// <returns>A result with a transformed success type</returns>
    public static Rs<U> Map<T, U>(this Rs<T> res, Func<T, U> func)
        => res.IsOk
        ? func(res.Value!)
        : res.Error!.ToFailRs<U>();

    /// <summary>
    /// Transforms a result type into another result type, using
    /// the specified function to convert the successful value
    /// into another result. If the converted result is
    /// a failure, then that failure will be passed on to the to
    /// returned result.
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <typeparam name="U">The success type of the transformed result</typeparam>
    /// <param name="res">result to map</param>
    /// <param name="func">function that operates on the success value</param>
    /// <returns>A result with a transformed success type</returns>
    public static Rs<U> FlatMap<T, U>(this Rs<T> res, Func<T, Rs<U>> func)
        => res.IsOk
        ? func(res.Value!)
        : res.Error!.ToFailRs<U>();

    /// <summary>
    /// Returns the success value if the result is a success.
    /// Returns the specified fallback value if the result is an error.
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="res">result to unpack</param>
    /// <param name="fallback">Fallback value to use if the result is an error</param>
    /// <returns>The success value or the fallback value</returns>
    public static T OrElse<T>(this Rs<T> res, T fallback)
        => res.IsOk
        ? res.Value!
        : fallback;


    /// <summary>
    /// Returns the success value if the result is a success.
    /// Invokes the fallback producer and returns its value if the result is an error.
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="res">result to unpack</param>
    /// <param name="fallbackProducer">Function that is invoked to produce the fallback value</param>
    /// <returns>The success value or the value produced by the fallback producer</returns>
    public static T OrElseThen<T>(this Rs<T> res, Func<T> fallbackProducer)
        => res.IsOk
        ? res.Value!
        : fallbackProducer.Invoke();


    /// <summary>
    /// Returns whether or not the result is a success. Use in an if-else clause to
    /// safely access the value or error in the corresponding block.
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="res">Result to fork</param>
    /// <param name="value">Value that is valid if the result is a success</param>
    /// <param name="er">Error that is valid if the result is failure</param>
    /// <returns>True if the result is a success, false if it is a failure</returns>
    public static bool Fork<T>(this Rs<T> res, [NotNullWhen(true)] out T? value, [NotNullWhen(false)] out IEr? er)
    {
        value = res.Value;
        er = res.Error;
        return res.IsOk;
    }

    /// <summary>
    /// Returns whether or not the result is a success. Use in an if clause to
    /// safely access the value in the following block.
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="res">Result to test</param>
    /// <param name="value">Value that is valid if the result is a success</param>
    /// <returns>True if the result is a success, false if it is a failure</returns>
    public static bool Then<T>(this Rs<T> res, [NotNullWhen(true)] out T? value)
    {
        value = res.Value;
        return res.IsOk;
    }

    /// <summary>
    /// Returns whether or not the result is a failure. Use in an if clause to
    /// safely access the value outside of the following block.
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="res">Result to test</param>
    /// <param name="value">Value that is valid if the result is a success</param>
    /// <returns>True if the result is a success, false if it is a failure</returns>
    public static bool ThenDont<T>(this Rs<T> res, [NotNullWhen(false)] out T? value)
    {
        value = res.Value;
        return res.IsEr;
    }


    /// <summary>
    /// Returns whether or not the result is a failure. Use in an if clause
    /// to safely access the underlying error in the following block.
    /// </summary>
    /// <typeparam name="T">The success type</typeparam>
    /// <param name="res">Result to falsify</param>
    /// <param name="er">Error that is valid if the result is a success</param>
    /// <returns>True if the result is a failure, false if it is a success</returns>
    public static bool Fail<T>(this Rs<T> res, [NotNullWhen(true)] out IEr? er)
    {
        er = res.Error;
        return res.IsEr;
    }
}