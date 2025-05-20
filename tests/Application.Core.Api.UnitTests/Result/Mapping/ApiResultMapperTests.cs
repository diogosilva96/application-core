using System.Diagnostics.CodeAnalysis;
using Application.Core.Api.Result;
using Application.Core.Api.Result.Mapping;
using Application.Core.Api.UnitTests.Result.Mapping.Utils;
using AutoFixture;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Application.Core.Api.UnitTests.Result.Mapping;

[SuppressMessage("Usage", "xUnit1045:Avoid using TheoryData type arguments that might not be serializable")]
public class ApiResultMapperTests
{
    private readonly ApiResultMapper _mapper = new();

    [Theory]
    [ClassData(typeof(TestData))]
    public void Map_MapsToExpectedResult(ApiResult apiResult, IResult expectedResult)
    {
        // Act
        var result = _mapper.Map(apiResult);

        // Assert
        Assert.IsType(expectedResult.GetType(), result);
        Assert.Equivalent(expectedResult, result);
    }
    
    [Fact]
    public void Map_ThrowsInvalidOperationException_WhenResultBeingMappedIsNotHandled()
    {
        // Arrange
        var apiResult = new TestSuccess(); // This is a custom result type that is not handled by the mapper.
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mapper.Map(apiResult));
    }
    
    private class TestData : TheoryData<ApiResult, IResult>
    {
        public TestData()
        {
            var fixture = new Fixture();
            var value = fixture.Create<TestValue>();
            var uri = fixture.Create<Uri>();
            
            Add(new Ok(value), Results.Ok((object)value));
            Add(new Created(value, uri), Results.Created(uri, (object)value));
            Add(new NoContent(), Results.NoContent());
            Add(new Accepted(value, uri), Results.Accepted(uri.ToString(), (object)value));
            
            var notFound = new NotFound(fixture.Create<string>());
            Add(notFound, Results.Problem(notFound.Detail, notFound.Instance, notFound.Status, notFound.Title, notFound.Type, notFound.Extensions));
           
            var badRequest = new BadRequest(fixture.Create<string>(), fixture.CreateMany<ValidationFailure>())
            {
                Detail = fixture.Create<string>(),
                Instance = fixture.Create<string>(),
                Title = fixture.Create<string>(),
                Type = fixture.Create<string>(),
                Extensions = new Dictionary<string, object?>
                {
                    { "custom-key", fixture.Create<string>() }
                }
            };
            Add(badRequest, Results.Problem(badRequest.Detail, badRequest.Instance, badRequest.Status, badRequest.Title, badRequest.Type, badRequest.Extensions));
            
            var unauthorized = new Unauthorized(fixture.Create<string>())
            {
                Detail = fixture.Create<string>(),
                Instance = fixture.Create<string>(),
                Title = fixture.Create<string>(),
                Type = fixture.Create<string>(),
                Extensions = new Dictionary<string, object?>
                {
                    { "custom-key", fixture.Create<string>() }
                }
            };
            Add(unauthorized, Results.Problem(unauthorized.Detail, unauthorized.Instance,unauthorized.Status, unauthorized.Title, unauthorized.Type, unauthorized.Extensions));
        }
    }
}