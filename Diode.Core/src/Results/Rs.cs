namespace Diode;

/// <summary>
/// A monad that either carries a successful value or an error
/// that can have an error message.
/// </summary>
/// 
/// <typeparam name="T">
/// The type of value that is stored in the result, IF
/// the result is a success.
/// </typeparam>
/// 
/// <param name="IsOk">
/// A Boolean indicating whether or not this result is a success
/// </param>
/// 
/// <param name="Value">
/// The value contained in the result, IF the result is a success.
/// </param>
/// 
/// <param name="Error">
/// The error contained in the result, IF the result is in fact
/// an error.
/// </param>
/// 
public readonly record struct Rs<T>(bool IsOk, T? Value, IEr? Error)
{
    // // // properties
    /// <summary>
    /// A Boolean indicating whether or not this result is NOT a success.
    /// </summary>
    public bool IsEr => !IsOk;

    // // // operators
    /// <summary>
    /// Implicitly converts the success type to a success result
    /// </summary>
    /// <param name="Value">The success value to convert</param>
    public static implicit operator Rs<T>(T Value) => new(true, Value, null);

    /// <summary>
    /// Implicitly converts the error into a failure result.
    /// </summary>
    /// <param name="Error">The error value to convert</param>
    public static implicit operator Rs<T>(Er Error) => new(false, default, Error);

    /// <summary>
    /// Implicitly converts the error into a failure result.
    /// </summary>
    /// <param name="Error">The error value to convert</param>
    public static implicit operator Rs<T>(PEr Error) => new(false, default, Error);

    // // // methods

    /// <summary>
    /// Performs a side effect with the successful, only if the result is a success
    /// </summary>
    /// <param name="sideEffect">Action to perform with the success value</param>
    /// <returns>The result, unaffected</returns>
    public Rs<T> OkTap(Action<T> sideEffect)
    {
        if (IsOk)
            sideEffect(Value!);
        return this;
    }

    /// <summary>
    /// Performs a side effect with the error, only if the result is an error
    /// </summary>
    /// <param name="sideEffect">Action to perform with the error</param>
    /// <returns>The result, unaffected</returns>
    public Rs<T> ErTap(Action<IEr> sideEffect)
    {
        if (IsEr)
            sideEffect(Error!);
        return this;
    }

    /// <summary>
    /// Returns the success value if the result is a success, or throws with 
    /// the error message if the result is an error.
    /// </summary>
    /// <returns>The success value</returns>
    /// <exception cref="Exception">Thrown if the result is an error</exception>
    public T Unwrap() => this.Fork(out T? value, out IEr? err)
        ? value
        : throw new Exception(err.Msg);
}
