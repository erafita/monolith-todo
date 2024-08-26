namespace Todo.Domain.Tests;

public class TodoItemTests
{
    private readonly UserId userId = new UserId(Guid.NewGuid());

    [Fact]
    public void CreateTodoItem_Should_ThrowException_WhenDescriptionIsEmpty()
    {
        // Act
        Func<TodoItem> fnc = () => TodoItem.Create(userId, string.Empty);

        // Assert
        fnc.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateTodoItem_Should_Success()
    {
        // Act
        var todoItem = TodoItem.Create(userId, "Description");

        // Assert
        todoItem.DueDate.Should().BeNull();
        todoItem.Labels.Should().BeEmpty();
        todoItem.UserId.Should().Be(userId);
        todoItem.IsCompleted.Should().BeFalse();
        todoItem.Priority.Should().Be(Priority.Normal);
        todoItem.Description.Should().Be("Description");
    }

    [Fact]
    public void UpdateTodoItem_Should_ThrowException_WhenDescriptionIsEmpty()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .Build();

        // Act
        Action act = () => todoItem.UpdateDescription(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateTodoItem_Description_Should_Success()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .Build();

        // Act
        todoItem.UpdateDescription("Description");

        // Assert
        todoItem.Description.Should().Be("Description");
    }

    [Fact]
    public void UpdateTodoItem_DueDate_Should_Success()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .Build();

        DateTime dueDate = DateTime.UtcNow;

        // Act
        todoItem.SetDueDate(dueDate);

        // Assert
        todoItem.DueDate.Should().Be(dueDate);
    }

    [Fact]
    public void UpdateTodoItem_Priority_Should_Success()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .Build();

        // Act
        todoItem.SetPriority(Priority.High);

        // Assert
        todoItem.Priority.Should().Be(Priority.High);
    }

    [Fact]
    public void TodoItem_SetAsCompleted_Should_Success()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .Build();

        // Act
        todoItem.SetCompleted(true, DateTime.UtcNow);

        // Assert
        todoItem.IsCompleted.Should().BeTrue();
        todoItem.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public void TodoItem_AddLabel_ShouldFail_WhenLabelAlreadyExists()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .WithLabels(["New Label 1", "New Label 2"])
            .Build();

        // Act
        todoItem.AddLabel("New Label 1");

        // Assert
        todoItem.Labels.Should().HaveCount(2);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void TodoItem_AddLabel_ShouldFail_WhenLabelIsEmpty(string label)
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .WithLabels(["New Label 1", "New Label 2"])
            .Build();

        // Act
        todoItem.AddLabel(label);

        // Assert
        todoItem.Labels.Should().HaveCount(2);
    }

    [Fact]
    public void TodoItem_AddLabel_Should_Success()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .Build();

        // Act
        todoItem.AddLabel("New Label 1");
        todoItem.AddLabel("New Label 2");

        // Assert
        todoItem.Labels.Should().Contain(["New Label 1", "New Label 2"]);
    }

    [Fact]
    public void TodoItem_DeleteLabel_ShouldFail_WhenLabelDoesNotExists()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .WithLabels(["New Label 1", "New Label 2"])
            .Build();

        // Act
        todoItem.DeleteLabel("New Label 3");

        // Assert
        todoItem.Labels.Should().HaveCount(2);
    }

    [Fact]
    public void TodoItem_DeleteLabel_Should_Success()
    {
        // Arrange
        TodoItem todoItem = TodoItemBuilder
            .Empty
            .WithLabels(["New Label 1", "New Label 2"])
            .Build();

        // Act
        todoItem.DeleteLabel("New Label 1");

        // Assert
        todoItem.Labels.Should().HaveCount(1);
    }
}
