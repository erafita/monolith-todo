namespace Todo.Application.Users.GetByEmail;

public sealed record GetUserByEmailQuery(string Email)
    : IQuery<UserResponse>;
