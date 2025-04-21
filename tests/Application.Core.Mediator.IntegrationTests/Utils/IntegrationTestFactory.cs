using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.IntegrationTests.Utils;

internal class IntegrationTestFactory : IDisposable
{
    private readonly ServiceCollection _serviceCollection = [];

    public ServiceProvider ServiceProvider { get; private set; } = null!;

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }

    public IntegrationTestFactory ConfigureServices(Action<IServiceCollection> configureServices)
    {
        configureServices(_serviceCollection);

        return this;
    }

    public void Build()
    {
        ServiceProvider = _serviceCollection.BuildServiceProvider();
    }

    public ISender CreateSender()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (ServiceProvider is null)
        {
            Build();
        }

        return ServiceProvider!.GetRequiredService<ISender>();
    }
}