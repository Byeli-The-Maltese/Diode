using System.Reactive.Linq;

namespace Diode;

/// <summary>
/// Extension methods that make it more ergonomic to work with observables
/// that emit <see cref="Rs{T}"/> values.
/// </summary>
/// 
/// <remarks>
/// These extensions provide common filtering, mapping, and side-effect
/// operations that respect the success-or-error semantics of
/// <see cref="Rs{T}"/> while operating within an observable sequence.
/// </remarks>
public static class RsObservable
{
    /// <summary>
    /// Filters an observable sequence to only successful results and
    /// extracts their success values.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The type of value stored in the result.
    /// </typeparam>
    /// 
    /// <param name="obs">
    /// The observable sequence of results to filter.
    /// </param>
    /// 
    /// <returns>
    /// An observable sequence containing only the success values of
    /// results that are successes.
    /// </returns>
    public static IObservable<T> WhereOk<T>(this IObservable<Rs<T>> obs)
        => obs
        .Where(r => r.IsOk)
        .Select(r => r.Value!);

    /// <summary>
    /// Filters an observable sequence to only error results and
    /// extracts their errors.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The type of value stored in the result.
    /// </typeparam>
    /// 
    /// <param name="obs">
    /// The observable sequence of results to filter.
    /// </param>
    /// 
    /// <returns>
    /// An observable sequence containing only the errors of
    /// results that are errors.
    /// </returns>
    public static IObservable<IEr> WhereEr<T>(this IObservable<Rs<T>> obs)
        => obs
        .Where(r => r.IsEr)
        .Select(r => r.Error!);

    /// <summary>
    /// Maps the success value of each result in an observable sequence
    /// using the provided function.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The type of the input success value.
    /// </typeparam>
    /// 
    /// <typeparam name="U">
    /// The type of the output success value.
    /// </typeparam>
    /// 
    /// <param name="obs">
    /// The observable sequence of results to map.
    /// </param>
    /// 
    /// <param name="func">
    /// The mapping function to apply to the success value,
    /// IF the result is a success.
    /// </param>
    /// 
    /// <returns>
    /// An observable sequence of mapped results.
    /// </returns>
    public static IObservable<Rs<U>> SelectMap<T, U>(this IObservable<Rs<T>> obs, Func<T, U> func)
        => obs
        .Select(r => r.Map(func));

    /// <summary>
    /// Maps the success value of each result in an observable sequence
    /// to another result, and flattens the outcome.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The type of the input success value.
    /// </typeparam>
    /// 
    /// <typeparam name="U">
    /// The type of the output success value.
    /// </typeparam>
    /// 
    /// <param name="obs">
    /// The observable sequence of results to map.
    /// </param>
    /// 
    /// <param name="func">
    /// The mapping function to apply to the success value,
    /// IF the result is a success.
    /// </param>
    /// 
    /// <returns>
    /// An observable sequence of flattened results.
    /// </returns>
    public static IObservable<Rs<U>> SelectFlatMap<T, U>(this IObservable<Rs<T>> obs, Func<T, Rs<U>> func)
        => obs
        .Select(r => r.FlatMap(func));

    /// <summary>
    /// Performs a side effect with the success value of each result in
    /// an observable sequence, only if the result is a success.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The type of value stored in the result.
    /// </typeparam>
    /// 
    /// <param name="obs">
    /// The observable sequence of results.
    /// </param>
    /// 
    /// <param name="sideEffect">
    /// Action to perform with the success value.
    /// </param>
    /// 
    /// <returns>
    /// The observable sequence of results, unaffected.
    /// </returns>
    public static IObservable<Rs<T>> DoIfOk<T>(this IObservable<Rs<T>> obs, Action<T> sideEffect)
        => obs
        .Do(r => r.OkTap(sideEffect));

    /// <summary>
    /// Performs a side effect with the error of each result in
    /// an observable sequence, only if the result is an error.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The type of value stored in the result.
    /// </typeparam>
    /// 
    /// <param name="obs">
    /// The observable sequence of results.
    /// </param>
    /// 
    /// <param name="sideEffect">
    /// Action to perform with the error.
    /// </param>
    /// 
    /// <returns>
    /// The observable sequence of results, unaffected.
    /// </returns>
    public static IObservable<Rs<T>> DoIfEr<T>(this IObservable<Rs<T>> obs, Action<IEr> sideEffect)
        => obs
        .Do(r => r.ErTap(sideEffect));
}
