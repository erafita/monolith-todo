namespace Todo.Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler(IUserRepository repository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        User? user = await repository.GetByIdAsync(new UserId(query.UserId), cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
        }

        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            LastName = user.LastName,
            FirstName = user.FirstName
        };
    }
}
