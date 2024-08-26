namespace Todo.Infrastructure.Todos;

internal sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("todos");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(t => t.Value, value => new TodoItemId(value));

        builder.Property(t => t.UserId)
            .HasConversion(t => t.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.Labels)
            .IsRequired(false);

        builder.Property(t => t.IsCompleted)
            .IsRequired();

        builder.Property(t => t.Priority)
            .IsRequired();

        builder.Property(t => t.CompletedAt)
            .HasConversion(d => d != null ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v)
            .IsRequired(false);

        builder.Property(t => t.DueDate)
            .HasConversion(d => d != null ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v)
            .IsRequired(false);

        builder.Property(t => t.CreatedAt)
            .HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .HasConversion(d => d != null ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v)
            .IsRequired(false);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId);
    }
}
