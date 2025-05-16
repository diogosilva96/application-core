namespace Application.Core.Result.UnitTests.Utils;

internal class TestError(string message) : IError
{
    public string ToDetailedErrorMessage() => message;
}