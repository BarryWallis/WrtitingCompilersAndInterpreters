
namespace FrontendComponents.Pascal;

/// <summary>
/// Represents an exception for unsupported languages. 
/// </summary>
[Serializable]
public class UnsupportedLanguageException : Exception
{
    /// <summary>
    /// Represents an exception for unsupported programming languages.
    /// </summary>
    /// <param name="language">Specifies the programming language that is not supported.</param>
    public UnsupportedLanguageException(string? language) : base($"Unsupported language: {language}")
    {
    }

    /// <summary>
    /// Represents an exception for unsupported programming languages.
    /// </summary>
    /// <param name="innerException">
    /// Holds additional information about a previous exception that led to 
    /// this error.
    /// </param>
    public UnsupportedLanguageException(string? language, Exception? innerException)
        : base(language, innerException)
    {
    }
}