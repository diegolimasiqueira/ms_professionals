using MSProfessionals.Application.Common;

namespace MSProfessionals.UnitTests.Common;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        // Arrange
        var value = "test value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Success_WithNullValue_ShouldCreateSuccessResult()
    {
        // Act
        var result = Result<string>.Success(null);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        // Arrange
        var error = "test error";

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(default, result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_WithNullError_ShouldCreateFailureResult()
    {
        // Act
        var result = Result<string>.Failure(null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(default, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Success_WithComplexType_ShouldCreateSuccessResult()
    {
        // Arrange
        var value = new TestClass { Id = 1, Name = "test" };

        // Act
        var result = Result<TestClass>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_WithComplexType_ShouldCreateFailureResult()
    {
        // Arrange
        var error = "test error";

        // Act
        var result = Result<TestClass>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(default, result.Value);
        Assert.Equal(error, result.Error);
    }

    private class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
} 