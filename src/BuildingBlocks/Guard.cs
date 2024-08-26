namespace BuildingBlocks;

public static class Guard
{
    public static string NotEmpty(
        string? value,
        [CallerArgumentExpression(nameof(value))] string? argumentName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The value can't be null", argumentName);
        }

        return value;
    }

    public static Guid NotEmpty(
        Guid? value,
        [CallerArgumentExpression(nameof(value))] string? argumentName = null)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("The value can't be null", argumentName);
        }

        return value!.Value;
    }

    public static DateTime NotEmpty(
        DateTime? value,
        [CallerArgumentExpression(nameof(value))] string? argumentName = null)
    {
        if (value == default)
        {
            throw new ArgumentException("The value can't be null", argumentName);
        }

        return value!.Value;
    }

    public static T NotNull<T>(
        T? value,
        [CallerArgumentExpression(nameof(value))] string? argumentName = null)
    {
        if (value is null)
        {
            throw new ArgumentException("The value can't be null", argumentName);
        }

        return value;
    }
}
