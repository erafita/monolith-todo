namespace ArchitectureTests;

public class CleanArchitectureTests : BaseTest
{
    [Fact]
    public void Domain_Should_NotHaveDependencyOnApplication()
    {
        // Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny("Application")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        // Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(InfrastructureAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        // Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(PresentationAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny(InfrastructureAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        // Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny(PresentationAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        // Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOnAny(PresentationAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
