namespace BuildingBlocks.ValueObjects;

public static class EmailErrors
{
    public static Error NullOrEmpty => Error.Failure(
        "Email.NullOrEmpty",
        "The email is required.");

    public static Error LongerThanAllowed => Error.Failure(
        "Email.LongerThanAllowed",
        "The email is longer than allowed.");

    public static Error InvalidFormat => Error.Failure(
        "Email.InvalidFormat",
        "The email format is invalid.");
}
