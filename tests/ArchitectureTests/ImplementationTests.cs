namespace ArchitectureTests;

public class ImplementationTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        // Arrange
        IEnumerable<IType> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .GetTypes();

        // Act
        var failingTypes = entityTypes
            .Where(entityType => !entityType
                .ReflectionType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                               .Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            .ToList();

        // Assert
        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_ShouldNotHave_PublicConstructor()
    {
        // Arrange
        IEnumerable<IType> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .GetTypes();

        // Act
        var failingTypes = entityTypes
            .Where(entityType => entityType
                .ReflectionType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                               .Any(c => c.IsPublic))
            .ToList();

        // Assert
        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_ShouldHave_StaticMethod_Create()
    {
        // Arrange
        IEnumerable<IType> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .GetTypes();

        // Act
        var failingTypes = entityTypes
            .Where(entityType => entityType
                .ReflectionType.GetMethod("Create", BindingFlags.Public | BindingFlags.Static) == null)
            .ToList();

        // Assert
        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateSetMethods()
    {
        // Arrange
        IEnumerable<IType> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .GetTypes();

        // Act
        var failingTypes = entityTypes
            .Where(entityType => entityType.ReflectionType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.SetMethod is not null)
                .Any(property => property.SetMethod!.IsPublic && !property.IsInitOnly()))
            .ToList();

        // Assert
        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Commands_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_Should_BePublic()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .BePublic()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlers_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlers_Should_BeInternal()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Should()
            .BeInternal()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Queries_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Queries_Should_BePublic()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .BePublic()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_Should_BeInternal()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .BeInternal()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Validators_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Validators_Should_BeInternal()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .BeInternal()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void EventHandlers_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(INotificationHandler<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void EntityTypeConfigurations_Should_BeInternal()
    {
        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .Should()
            .BeInternal()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Repositories_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IRepository<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Repositories_Should_BeInternal()
    {
        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IRepository<>))
            .Should()
            .BeInternal()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void EndPoints_Should_BeSealed()
    {
        // Act
        TestResult result = Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void EndPoints_Should_BeInternal()
    {
        // Act
        TestResult result = Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .BeInternal()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
