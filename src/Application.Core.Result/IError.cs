namespace Application.Core.Result;

/// <summary>
/// Represents an error abstraction.
/// </summary>
public interface IError
{
    /// <summary>
    /// Builds a detailed message for the error.
    /// </summary>
    /// <returns>A string containing a detailed message of the error.</returns>
    public string ToDetailedErrorMessage();
}