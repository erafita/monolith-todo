namespace Todo.Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "The user with the specified email address was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email address is not unique");

    public static readonly Error InvalidEmailOrPassword = Error.NotFound(
        "Users.InvalidEmailOrPassword",
        "The provided email address or password are not valid");

    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");
}
