
namespace FrontendComponents.Pascal;

/// <summary>
/// Represents an exception for unsupported parser types.
/// </summary>
[Serializable]
public class UnsupportedParserTypeException : Exception
{
    /// <summary>
    /// Exception thrown when an unsupported parser type is encountered.
    /// </summary>
    /// <param name="type">
    /// A string representing the unsupported parser type that triggered the exception.
    /// </param>
    public UnsupportedParserTypeException(string? type) : base($"Unsupported parser type: {type}")
    {
    }

    /// <summary>
    /// Represents an exception for unsupported parser types.
    /// </summary>
    /// <param name="type">Specifies the unsupported parser type that triggered the exception.</param>
    /// <param name="innerException">
    /// Holds an optional exception that caused this exception to be thrown.
    /// </param>
    public UnsupportedParserTypeException(string? type, Exception? innerException)
        : base($"Unsupported parser type: {type}", innerException)
    {
    }
}