namespace Todo.Infrastructure.Users;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Ignore(u => u.DomainEvents);

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(u => u.Value, value => new UserId(value));

        builder.Property(u => u.FirstName)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(255);

        builder.Property(u => u.Password)
            .HasMaxLength(128)
            .IsRequired();

        builder.ComplexProperty(
            u => u.Email,
            u => u.Property(email => email.Value)
                  .HasConversion(email => email, value => Email.Create(value).Value)
                  .HasColumnName(nameof(User.Email))
                  .HasMaxLength(Email.MaxLength)
            .IsRequired());
    }
}
