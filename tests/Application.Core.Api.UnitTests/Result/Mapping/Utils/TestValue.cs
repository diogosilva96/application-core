namespace Application.Core.Api.UnitTests.Result.Mapping.Utils;

internal record TestValue
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
}