using Microsoft.Extensions.DependencyInjection;
using MSProfessionals.Application;
using MediatR;
using Xunit;

namespace MSProfessionals.UnitTests.Application;

public class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_ShouldRegisterMediatR()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verifica se o MediatR foi registrado corretamente
        Assert.NotNull(serviceProvider.GetService<IMediator>());
        Assert.NotNull(serviceProvider.GetService<ISender>());
        Assert.NotNull(serviceProvider.GetService<IPublisher>());
    }

    [Fact]
    public void AddApplication_ShouldRegisterMediatRWithCorrectLifetime()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IMediator));
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);

        serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ISender));
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);

        serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPublisher));
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }
} 