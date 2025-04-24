using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.UnitTests.Builder;

internal class SenderBuilder
{
    private IServiceProvider _serviceProvider = new ServiceCollection().BuildServiceProvider();
    private ConcurrentDictionary<Type, MethodInfo> _methodCache = new();
    
    public Sender Build() => new(_serviceProvider, _methodCache);

    public SenderBuilder With(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        return this;
    }

    public SenderBuilder With(ConcurrentDictionary<Type, MethodInfo> methodCache)
    {
        _methodCache = methodCache;

        return this;
    }
}