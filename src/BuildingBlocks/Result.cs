namespace BuildingBlocks;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result<TValue> Create<TValue>(TValue value) where TValue : class
        => value is null ? Failure<TValue>(Error.None) : Success(value);

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);
}

public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    private readonly TValue? value = value;

    [NotNull]
    public TValue Value => IsSuccess
        ? value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<TValue> ValidationFailure(Error error) =>
        new(default, false, error);
}
