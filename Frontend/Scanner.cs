namespace FrontendLib;

/// <summary>
/// A language-independent framework class that provides a base for language-specific scanner implementations.
/// </summary>
/// <remarks>
/// This abstract class defines the interface for tokenizing source code. Subclasses must implement
/// the <see cref="ExtractToken"/> method to provide language-specific token extraction logic.
/// </remarks>
public abstract class Scanner(Source theSource)
{
    protected Source source = theSource;     // source
    private Token? _currentToken;  // current token

    /// <summary>
    /// Gets the current token.
    /// </summary>
    /// <returns>The current token.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the current token has not been initialized.</exception>
    public Token CurrentToken() => _currentToken ?? throw new InvalidOperationException($"{nameof(_currentToken)} has not been initialized.");

    /// <summary>
    /// Returns the next token from the source.
    /// </summary>
    /// <returns>The next token.</returns>
    public Token NextToken()
    {
        _currentToken = ExtractToken();
        return _currentToken;
    }

    /// <summary>
    /// Extracts and returns the next token from the source.
    /// </summary>
    /// <remarks>
    /// This method performs the actual work of tokenization and must be implemented by scanner subclasses
    /// to provide language-specific token extraction logic.
    /// </remarks>
    /// <returns>The next token.</returns>
    protected abstract Token ExtractToken();

    /// <summary>
    /// Gets the current character from the source.
    /// </summary>
    /// <returns>The current character from the source.</returns>
    public char CurrentChar() => source.CurrentChar();

    /// <summary>
    /// Gets the next character from the source.
    /// </summary>
    /// <returns>The next character from the source.</returns>
    public char NextChar() => source.NextChar();
}
