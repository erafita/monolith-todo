namespace Todo.Shared.Tests.Builders;

public sealed class TodoItemBuilder
{
    private readonly UserBuilder user = UserBuilder.Empty;

    private string description = "N/A";
    private DateTime? dueDate = default;
    private List<string> labels = [];
    private bool isCompleted = false;
    private DateTime? completedAt = default;
    private Priority priority = Priority.Normal;

    private TodoItemBuilder()
    { }

    public static TodoItemBuilder Empty =>
        new();

    public TodoItemBuilder WithUser(Action<UserBuilder> action)
    {
        action(user);
        return this;
    }

    public TodoItemBuilder WithDescription(string description)
    {
        this.description = description;
        return this;
    }

    public TodoItemBuilder WithDueDate(DateTime? dueDate)
    {
        this.dueDate = dueDate;
        return this;
    }

    public TodoItemBuilder WithLabels(List<string> labels)
    {
        this.labels = labels;
        return this;
    }

    public TodoItemBuilder WithIsCompleted(bool isCompleted)
    {
        this.isCompleted = isCompleted;

        if (this.isCompleted)
        {
            completedAt = DateTime.UtcNow;
        }

        return this;
    }

    public TodoItemBuilder WithPriority(Priority priority)
    {
        this.priority = priority;
        return this;
    }

    public TodoItem Build()
    {
        var todoItem = TodoItem.Create(
            user.Build().Id,
            description,
            dueDate,
            priority,
            labels);

        todoItem.SetCompleted(isCompleted, completedAt);

        return todoItem;
    }
}
