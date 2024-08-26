namespace Todo.Domain.Todos;

public sealed class TodoItem : Entity<TodoItemId>, IAuditableEntity
{
    private TodoItem(
        UserId userId,
        string description,
        DateTime? dueDate = null,
        Priority? priority = null,
        List<string>? labels = null)
    {
        UserId = userId;
        DueDate = dueDate;
        IsCompleted = false;
        Labels = labels ?? [];
        Id = new TodoItemId(Guid.NewGuid());
        Priority = priority ?? Priority.Normal;
        Description = Guard.NotEmpty(description);
    }

    private TodoItem()
    { }

    public UserId UserId { get; init; } = default!;
    public string Description { get; private set; } = default!;
    public DateTime? DueDate { get; private set; } = default!;
    public List<string> Labels { get; private set; } = default!;
    public bool IsCompleted { get; private set; } = default!;
    public DateTime? CompletedAt { get; private set; } = default!;
    public Priority Priority { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; } = default!;
    public DateTime? UpdatedAt { get; private set; } = default!;

    public static TodoItem Create(
        UserId userId,
        string description,
        DateTime? dueDate = null,
        Priority? priority = null,
        List<string>? labels = null) =>
            new(userId, description, dueDate, priority, labels);

    public void UpdateDescription(string description)
    {
        Description = Guard.NotEmpty(description);
    }

    public void SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
    }

    public void SetPriority(Priority priority)
    {
        Priority = priority;
    }

    public void SetCompleted(bool isCompleted, DateTime? completedAt)
    {
        if (IsCompleted != isCompleted)
        {
            IsCompleted = isCompleted;
            CompletedAt = completedAt;

            AddDomainEvent(new TodoItemCompletedDomainEvent(Id));
        }
    }

    public void AddLabel(string label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            return;
        }

        if (!Labels.Exists(l => l == label))
        {
            Labels.Add(label);
        }
    }

    public void DeleteLabel(string label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            return;
        }

        Labels.Remove(label);
    }
}
