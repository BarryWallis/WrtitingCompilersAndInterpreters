namespace FrontendLib;

/// <summary>
/// Base scanner abstraction that reads characters from a <see cref="Source"/>
/// and extracts <see cref="Token"/> instances.
/// </summary>
public abstract class Scanner(Source source)
{
    // ---------------------------------------------------------------------
    // Fields
    // ---------------------------------------------------------------------

    protected Source _source = source ?? throw new ArgumentNullException(nameof(source));

    // ---------------------------------------------------------------------
    // Properties
    // ---------------------------------------------------------------------

    /// <summary>
    /// Gets the current token, if any, produced by the last call to <see cref="GetNextToken"/>.
    /// </summary>
    public Token? CurrentToken { get; private set; }

    /// <summary>
    /// Gets the current character from the underlying source.
    /// </summary>
    public char CurrentCharacter => _source.CurrentCharacter;

    // ---------------------------------------------------------------------
    // Public methods
    // ---------------------------------------------------------------------

    /// <summary>
    /// Extracts the next token from the source and sets <see cref="CurrentToken"/>.
    /// </summary>
    public Token GetNextToken()
    {
        CurrentToken = ExtractToken();
        return CurrentToken;
    }

    /// <summary>
    /// Extracts the next token. Implemented by concrete scanners.
    /// </summary>
    protected abstract Token ExtractToken();

    /// <summary>
    /// Advances the underlying <see cref="Source"/> to the next character.
    /// </summary>
    public char GetNextCharacter() => _source.GetNextCharacter();
}
