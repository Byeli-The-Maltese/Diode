namespace Diode;

/// <summary>
/// Extension methods that make it easier to combine and transform
/// results that carry tuples as their success values.
/// </summary>
/// 
/// <remarks>
/// These extensions provide tuple-aware variants of common result
/// operations, allowing multiple results to be combined and then
/// mapped or flat-mapped in a structured way.
/// </remarks>
public static class RsTuplify
{
    // // // And

    /// <summary>
    /// Combines two results into a single result containing a tuple
    /// of their success values, only if both results are successes.
    /// </summary>
    public static Rs<(T1, T2)> And<T1, T2>(this Rs<T1> r1, Rs<T2> r2)
        => r1.FlatMap(s1
            => r2.Fork(out var s2, out var e2)
            ? (s1, s2).ToRs<(T1, T2)>()
            : e2.ToFailRs<(T1, T2)>()
        );

    /// <summary>
    /// Combines three results into a single result containing a tuple
    /// of their success values, only if all results are successes.
    /// </summary>
    public static Rs<(T1, T2, T3)> And<T1, T2, T3>(
        this Rs<(T1, T2)> r,
        Rs<T3> r3)
        => r.FlatMap(s
            => r3.Fork(out var s3, out var e3)
            ? (s.Item1, s.Item2, s3).ToRs<(T1, T2, T3)>()
            : e3.ToFailRs<(T1, T2, T3)>()
        );

    /// <summary>
    /// Combines four results into a single result containing a tuple
    /// of their success values, only if all results are successes.
    /// </summary>
    public static Rs<(T1, T2, T3, T4)> And<T1, T2, T3, T4>(
        this Rs<(T1, T2, T3)> r,
        Rs<T4> r4)
        => r.FlatMap(s
            => r4.Fork(out var s4, out var e4)
            ? (s.Item1, s.Item2, s.Item3, s4).ToRs<(T1, T2, T3, T4)>()
            : e4.ToFailRs<(T1, T2, T3, T4)>()
        );

    /// <summary>
    /// Combines five results into a single result containing a tuple
    /// of their success values, only if all results are successes.
    /// </summary>
    public static Rs<(T1, T2, T3, T4, T5)> And<T1, T2, T3, T4, T5>(
        this Rs<(T1, T2, T3, T4)> r,
        Rs<T5> r5)
        => r.FlatMap(s
            => r5.Fork(out var s5, out var e5)
            ? (s.Item1, s.Item2, s.Item3, s.Item4, s5).ToRs<(T1, T2, T3, T4, T5)>()
            : e5.ToFailRs<(T1, T2, T3, T4, T5)>()
        );

    // // // TupleMap

    /// <summary>
    /// Maps the success value of a tuple result using the provided function.
    /// </summary>
    public static Rs<U> TupleMap<T1, T2, U>(
        this Rs<(T1, T2)> r,
        Func<T1, T2, U> func)
        => r.Map(s => func(s.Item1, s.Item2));

    /// <summary>
    /// Maps the success value of a tuple result using the provided function.
    /// </summary>
    public static Rs<U> TupleMap<T1, T2, T3, U>(
        this Rs<(T1, T2, T3)> r,
        Func<T1, T2, T3, U> func)
        => r.Map(s => func(s.Item1, s.Item2, s.Item3));

    /// <summary>
    /// Maps the success value of a tuple result using the provided function.
    /// </summary>
    public static Rs<U> TupleMap<T1, T2, T3, T4, U>(
        this Rs<(T1, T2, T3, T4)> r,
        Func<T1, T2, T3, T4, U> func)
        => r.Map(s => func(s.Item1, s.Item2, s.Item3, s.Item4));

    /// <summary>
    /// Maps the success value of a tuple result using the provided function.
    /// </summary>
    public static Rs<U> TupleMap<T1, T2, T3, T4, T5, U>(
        this Rs<(T1, T2, T3, T4, T5)> r,
        Func<T1, T2, T3, T4, T5, U> func)
        => r.Map(s => func(s.Item1, s.Item2, s.Item3, s.Item4, s.Item5));

    // // // TupleFlatMap

    /// <summary>
    /// Flat-maps the success value of a tuple result using the provided function.
    /// </summary>
    public static Rs<U> TupleFlatMap<T1, T2, U>(
        this Rs<(T1, T2)> r,
        Func<T1, T2, Rs<U>> func)
        => r.FlatMap(s => func(s.Item1, s.Item2));

    /// <summary>
    /// Flat-maps the success value of a tuple result using the provided function.
    /// </summary>
    public static Rs<U> TupleFlatMap<T1, T2, T3, U>(
        this Rs<(T1, T2, T3)> r,
        Func<T1, T2, T3, Rs<U>> func)
        => r.FlatMap(s => func(s.Item1, s.Item2, s.Item3));

    /// <summary>
    /// Flat-maps the success value of a tuple result using the provided function.
    /// </summary>
    public static Rs<U> TupleFlatMap<T1, T2, T3, T4, U>(
        this Rs<(T1, T2, T3, T4)> r,
        Func<T1, T2, T3, T4, Rs<U>> func)
        => r.FlatMap(s => func(s.Item1, s.Item2, s.Item3, s.Item4));

    /// <summary>
    /// Flat-maps the success value of a tuple result using the provided function.
    /// </summary>
    public static Rs<U> TupleFlatMap<T1, T2, T3, T4, T5, U>(
        this Rs<(T1, T2, T3, T4, T5)> r,
        Func<T1, T2, T3, T4, T5, Rs<U>> func)
        => r.FlatMap(s => func(s.Item1, s.Item2, s.Item3, s.Item4, s.Item5));
}
