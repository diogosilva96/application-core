using Application.Core.Api.RequestProcessing;
using Application.Core.Api.Result;
using Application.Core.Api.Result.Mapping;
using Application.Core.Api.UnitTests.Utils;
using Application.Core.Mediator;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Application.Core.Api.UnitTests.RequestProcessing;

public class ApiRequestProcessorTests
{
    private readonly Fixture _fixture;
    private readonly ApiRequestProcessor _processor;
    private readonly IApiResultMapper _resultMapper;
    private readonly IMediator _mediator;

    public ApiRequestProcessorTests()
    {
        _fixture = new();
        _mediator = Substitute.For<IMediator>();
        _resultMapper = Substitute.For<IApiResultMapper>();
        _processor = new(_mediator, _resultMapper, Substitute.For<ILogger<ApiRequestProcessor>>());
    }

    [Fact]
    public async Task ProcessAsync_ProcessesRequest()
    {
        // Arrange
        ApiResult expectedApiResult = new Ok(_fixture.Create<int>());
        var expectedResult = Results.Ok(_fixture.Create<int>());
        _mediator.HandleAsync(Arg.Any<TestRequest>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(expectedApiResult);
        _resultMapper.Map(Arg.Any<ApiResult>()).ReturnsForAnyArgs(expectedResult);
        var request = _fixture.Create<TestRequest>();

        // Act
        var result = await _processor.ProcessAsync(request, TestContext.Current.CancellationToken);

        // Assert
        await _mediator.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
        _resultMapper.Received(1).Map(expectedApiResult);
        Assert.Equal(expectedResult, result);
    }
}