namespace Todo.Web.Api.Endpoints.Todos;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("todos", async (Request request, IUserContext context, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateTodoCommand(
                context.UserId,
                request.Description,
                request.DueDate,
                request.Priority.HasValue ? (Priority)request.Priority : null,
                request.Labels);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags("Todos")
        .RequireAuthorization();
    }

    public sealed class Request
    {
        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string Description { get; set; }
        public List<string>? Labels { get; set; } = [];
    }
}
