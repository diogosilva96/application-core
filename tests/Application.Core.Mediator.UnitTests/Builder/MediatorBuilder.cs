using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.UnitTests.Builder;

internal class MediatorBuilder
{
    private IServiceProvider _serviceProvider = new ServiceCollection().BuildServiceProvider();
    private ConcurrentDictionary<Type, MethodInfo> _methodCache = new();
    
    public Mediator Build() => new(_serviceProvider, _methodCache);

    public MediatorBuilder With(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        return this;
    }

    public MediatorBuilder With(ConcurrentDictionary<Type, MethodInfo> methodCache)
    {
        _methodCache = methodCache;

        return this;
    }
}