namespace Application.Core.Result;

/// <summary>
/// Represents a result with a value of type <see cref="T:T"/> and an error of type <see cref="T:TError"/>.
/// </summary>
/// <typeparam name="T">The type for the value of the result.</typeparam>
/// <typeparam name="TError">The error type of the result.</typeparam>
// TODO: Consider supporting Result<TError> where TError : IError for a result without a value (void)
public record Result<T, TError> where TError : IError
{
    /// <summary>
    /// Creates a successful <see cref="Result{T, TError}" /> with the given <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <exception cref="ArgumentNullException">
    /// Exception thrown when the <paramref name="value"/> is not specified.
    /// </exception>
    public Result(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
        IsSuccess = true;
    }

    /// <summary>
    /// Creates an error <see cref="Result{T, TError}" /> with the given <paramref name="error" />.
    /// </summary>
    /// <param name="error">The error of the result.</param>
    /// <exception cref="ArgumentNullException">
    /// Exception thrown when the <paramref name="error"/> is not specified.
    /// </exception>
    public Result(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        Error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// Gets the value of the result.
    /// </summary>
    /// <remarks>Will be <c>null</c> when <see cref="IsSuccess" /> is <c>false</c>.</remarks>
    public T? Value { get; }

    /// <summary>
    /// Gets the error of the result.
    /// </summary>
    /// <remarks>Will be <c>null</c> when <see cref="IsSuccess" /> is <c>true</c>.</remarks>
    public TError? Error { get; }

    /// <summary>
    /// Says whether the result was successful.
    /// </summary>
    /// <remarks>
    /// If <c>true</c>, then the <see cref="Value" /> will be specified.
    /// If <c>false</c> then the <see cref="Error" /> will be specified.
    /// </remarks>
    public bool IsSuccess { get; }
    
    /// <summary>
    /// Says whether the result is an error.
    /// </summary>
    /// <remarks>
    /// If <c>false</c>, then the <see cref="Value" /> will be specified.
    /// If <c>true</c> then the <see cref="Error" /> will be specified.
    /// </remarks>
    public bool IsError => !IsSuccess;

    /// <summary>
    /// Matches the result to the given delegates.
    /// </summary>
    /// <param name="onSuccess">The delegate to execute when the result is successful.</param>
    /// <param name="onError">The delegate to execute when the result is an error.</param>
    /// <typeparam name="TResult">The matched result type.</typeparam>
    /// <returns>The matched result of type <see cref="T:TResult"/>.</returns>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<TError, TResult> onError)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onError);

        return IsSuccess ? onSuccess(Value!) : onError(Error!);
    }

    /// <summary>
    /// Matches the result to the given delegates asynchronously.
    /// </summary>
    /// <param name="onSuccess">The delegate to execute when the result is successful.</param>
    /// <param name="onError">The delegate to execute when the result is an error.</param>
    /// <typeparam name="TResult">The matched result type.</typeparam>
    /// <returns>The matched result of type <see cref="T:TResult"/>.</returns>
    public Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> onSuccess,
        Func<TError, Task<TResult>> onError)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onError);

        return IsSuccess ? onSuccess(Value!) : onError(Error!);
    }
    
    /// <summary>
    /// Switches the result to the given delegates.
    /// </summary>
    /// <param name="onSuccess">The action to execute when the result is successful.</param>
    /// <param name="onError">The action to execute when the result is an error.</param>
    public void Switch(
        Action<T> onSuccess,
        Action<TError> onError)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onError);

        if (IsSuccess)
        {
            onSuccess(Value!);
            return;
        }

        onError(Error!);
    }

    /// <summary>
    /// Switches the result to the given delegates asynchronously.
    /// </summary>
    /// <param name="onSuccess">The delegate to execute when the result is successful.</param>
    /// <param name="onError">The delegate to execute when the result is an error.</param>
    public Task SwitchAsync(
        Func<T, Task> onSuccess,
        Func<TError, Task> onError)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onError);

        return IsSuccess ? onSuccess(Value!) : onError(Error!);
    }
    
    public static implicit operator Result<T, TError>(T value) => new(value);
    public static implicit operator Result<T, TError>(TError error) => new(error);
}