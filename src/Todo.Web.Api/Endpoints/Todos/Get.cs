using Todo.Application.Todos.Get;

namespace Todo.Web.Api.Endpoints.Todos;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("todos", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetTodosQuery(userId);

            Result<List<TodoResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags("Todos")
        .RequireAuthorization();
    }
}
