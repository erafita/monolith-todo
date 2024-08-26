namespace Todo.Application.Users.GetByEmail;

internal sealed class GetUserByEmailQueryHandler(IUserRepository repository)
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(query.Email);

        if (email.IsFailure)
        {
            return Result.Failure<UserResponse>(email.Error);
        }

        User? user = await repository.GetByEmailAsync(email.Value, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
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
