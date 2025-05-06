using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using MSProfessionals.API.Extensions;
using MSProfessionals.API.Middleware;
using Xunit;

namespace MSProfessionals.UnitTests.Extensions;

public class ApplicationBuilderExtensionsTests
{
    [Fact]
    public void UseExceptionMiddleware_ShouldAddMiddlewareToPipeline()
    {
        // Arrange
        var appBuilder = new Mock<IApplicationBuilder>();
        var serviceProvider = new Mock<IServiceProvider>();
        var serviceScope = new Mock<IServiceScope>();
        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        var environment = new Mock<IWebHostEnvironment>();

        appBuilder.Setup(x => x.ApplicationServices).Returns(serviceProvider.Object);
        serviceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactory.Object);
        serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);
        serviceProvider.Setup(x => x.GetService(typeof(IWebHostEnvironment))).Returns(environment.Object);

        // Act
        appBuilder.Object.UseExceptionMiddleware();

        // Assert
        appBuilder.Verify(x => x.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.Once);
    }

    [Fact]
    public void UseExceptionMiddleware_ShouldThrowArgumentNullException_WhenAppBuilderIsNull()
    {
        // Arrange
        IApplicationBuilder appBuilder = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => appBuilder.UseExceptionMiddleware());
    }
} 