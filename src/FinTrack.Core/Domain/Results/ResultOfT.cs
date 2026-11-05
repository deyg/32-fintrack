using FinTrack.Core.Domain.Errors;

namespace FinTrack.Core.Domain.Results;

public sealed class Result<T>
{
    private readonly T? _value;
    private readonly Error? _error;

    private Result(bool isSuccess, T? value, Error? error)
    {
        IsSuccess = isSuccess;
        _value = value;
        _error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Um resultado de falha não possui valor.");
    public Error Error => IsFailure
        ? _error!
        : throw new InvalidOperationException("Um resultado de sucesso não possui erro.");

    public static Result<T> Success(T value) =>
        new(true, value is null ? throw new ArgumentNullException(nameof(value)) : value, null);

    public static Result<T> Failure(Error error) =>
        new(false, default, error ?? throw new ArgumentNullException(nameof(error)));

    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);
}
