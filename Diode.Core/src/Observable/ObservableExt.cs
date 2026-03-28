using System.Reactive.Linq;

namespace Diode;


/// <summary>
/// Extension methods that make it easier to work with observables
/// that emit tuple values.
/// </summary>
public static class ObservableExt
{
    /// <summary>
    /// Projects each tuple in an observable sequence into a new value
    /// using the provided function.
    /// </summary>
    public static IObservable<U> TupleSelect<T1, T2, U>(
        this IObservable<(T1, T2)> obs,
        Func<T1, T2, U> func)
        => obs.Select(t => func(t.Item1, t.Item2));

    /// <summary>
    /// Projects each tuple in an observable sequence into a new value
    /// using the provided function.
    /// </summary>
    public static IObservable<U> TupleSelect<T1, T2, T3, U>(
        this IObservable<(T1, T2, T3)> obs,
        Func<T1, T2, T3, U> func)
        => obs.Select(t => func(t.Item1, t.Item2, t.Item3));

    /// <summary>
    /// Projects each tuple in an observable sequence into a new value
    /// using the provided function.
    /// </summary>
    public static IObservable<U> TupleSelect<T1, T2, T3, T4, U>(
        this IObservable<(T1, T2, T3, T4)> obs,
        Func<T1, T2, T3, T4, U> func)
        => obs.Select(t => func(t.Item1, t.Item2, t.Item3, t.Item4));

    /// <summary>
    /// Projects each tuple in an observable sequence into a new value
    /// using the provided function.
    /// </summary>
    public static IObservable<U> TupleSelect<T1, T2, T3, T4, T5, U>(
        this IObservable<(T1, T2, T3, T4, T5)> obs,
        Func<T1, T2, T3, T4, T5, U> func)
        => obs.Select(t => func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5));

    /// <summary>
    /// Subscribes to an observable sequence of tuples and invokes
    /// the provided action for each emitted value.
    /// </summary>
    public static IDisposable TupleSubscribe<T1, T2>(
        this IObservable<(T1, T2)> obs,
        Action<T1, T2> action)
        => obs.Subscribe(t => action(t.Item1, t.Item2));

    /// <summary>
    /// Subscribes to an observable sequence of tuples and invokes
    /// the provided action for each emitted value.
    /// </summary>
    public static IDisposable TupleSubscribe<T1, T2, T3>(
        this IObservable<(T1, T2, T3)> obs,
        Action<T1, T2, T3> action)
        => obs.Subscribe(t => action(t.Item1, t.Item2, t.Item3));

    /// <summary>
    /// Subscribes to an observable sequence of tuples and invokes
    /// the provided action for each emitted value.
    /// </summary>
    public static IDisposable TupleSubscribe<T1, T2, T3, T4>(
        this IObservable<(T1, T2, T3, T4)> obs,
        Action<T1, T2, T3, T4> action)
        => obs.Subscribe(t => action(t.Item1, t.Item2, t.Item3, t.Item4));

    /// <summary>
    /// Subscribes to an observable sequence of tuples and invokes
    /// the provided action for each emitted value.
    /// </summary>
    public static IDisposable TupleSubscribe<T1, T2, T3, T4, T5>(
        this IObservable<(T1, T2, T3, T4, T5)> obs,
        Action<T1, T2, T3, T4, T5> action)
        => obs.Subscribe(t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5));
}
