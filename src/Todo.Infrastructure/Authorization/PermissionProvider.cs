namespace Todo.Infrastructure.Authorization;

internal sealed class PermissionProvider
{
    internal const string UsersAccess = "users:access";

    public Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        string id = userId.ToString();

        if (!string.IsNullOrWhiteSpace(id))
        {
            HashSet<string> permissionsSet = [UsersAccess];

            return Task.FromResult(permissionsSet);
        }

        return Task.FromResult(new HashSet<string>());
    }
}
