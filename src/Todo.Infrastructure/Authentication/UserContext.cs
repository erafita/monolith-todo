namespace Todo.Infrastructure.Authentication;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");
}
