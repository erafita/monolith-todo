using Todo.Application.Todos.GetById;

namespace Todo.Web.Api.Endpoints.Todos;

internal sealed class Complete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("todos/{id:guid}/complete", async (Guid id, IUserContext context, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetTodoByIdQuery(id);

            Result<TodoResponse> resultQuery = await sender.Send(query, cancellationToken);

            if (resultQuery is null)
            {
                Error error = TodoItemErrors.NotFound(query.TodoItemId);

                return Results.Problem(
                    title: error.Code,
                    detail: error.Description,
                    statusCode: StatusCodes.Status404NotFound,
                    type: "https://tools.ietf.org/html/rfc7231#section-6.5.4");
            }

            if (resultQuery.Value.UserId != context.UserId)
            {
                return Results.Forbid();
            }

            var command = new CompleteTodoCommand(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags("Todos")
        .RequireAuthorization();
    }
}
