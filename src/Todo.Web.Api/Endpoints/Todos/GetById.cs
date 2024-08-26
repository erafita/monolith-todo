using Todo.Application.Todos.GetById;

namespace Todo.Web.Api.Endpoints.Todos;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("todos/{id:guid}", async (Guid id, IUserContext context, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetTodoByIdQuery(id);

            Result<TodoResponse> result = await sender.Send(query, cancellationToken);

            if (result is not null && result.Value.UserId != context.UserId)
            {
                return Results.Forbid();
            }

            return result!.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags("Todos")
        .RequireAuthorization();
    }
}
