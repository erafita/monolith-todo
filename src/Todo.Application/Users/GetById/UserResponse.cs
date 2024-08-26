namespace Todo.Application.Users.GetById;

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; } = default!;

    public string FirstName { get; init; } = default!;

    public string? LastName { get; init; }
}
