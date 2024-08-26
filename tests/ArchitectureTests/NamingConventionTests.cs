namespace ArchitectureTests;

public class NamingConventionTests : BaseTest
{
    [Fact]
    public void IRepositories_ShouldHave_NameEndingWith_Repository()
    {
        // Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IRepository<>))
            .Should()
            .BeInterfaces()
            .And()
            .HaveNameEndingWith("Repository")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEvents_ShouldHave_NameEndingWith_DomainEvent()
    {
        // Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_ShouldHave_NameEndingWith_Command()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlers_ShouldHave_NameEndingWith_CommandHandler()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Queries_ShouldHave_NameEndingWith_Query()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_ShouldHave_NameEndingWith_QueryHandler()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Validators_ShouldHave_NameEndingWith_Validator()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void EventHandlers_ShouldHave_NameEndingWith_EventHandler()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(INotificationHandler<>))
            .Should()
            .HaveNameEndingWith("EventHandler")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void EntityTypeConfigurations_ShouldHave_NameEndingWith_Configuration()
    {
        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .Should()
            .HaveNameEndingWith("Configuration")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Repositories_ShouldHave_NameEndingWith_Repository()
    {
        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IRepository<>))
            .Should()
            .HaveNameEndingWith("Repository")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
