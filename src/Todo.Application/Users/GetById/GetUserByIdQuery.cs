namespace Todo.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId)
    : IQuery<UserResponse>;
