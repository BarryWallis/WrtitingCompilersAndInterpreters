namespace FrontendLib;

/// <summary>
/// Base token representation produced by scanners.
/// </summary>
public class Token
{
    // ---------------------------------------------------------------------
    // Fields
    // ---------------------------------------------------------------------

    protected ITokenType? _type = null;
    protected string? _text = null;
    protected object? _value = null;
    protected Source _source;
    protected int _lineNumber;
    protected int _position;

    // ---------------------------------------------------------------------
    // Properties
    // ---------------------------------------------------------------------

    /// <summary>
    /// Gets the current character from the underlying source.
    /// </summary>
    protected char CurrentCharacter => _source.CurrentCharacter;

    /// <summary>
    /// Gets the 1-based line number associated with this token.
    /// </summary>
    public int LineNumber => _lineNumber;

    // ---------------------------------------------------------------------
    // Constructors
    // ---------------------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class and extracts its content from the source.
    /// </summary>
    public Token(Source source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _lineNumber = source.LineNumber;
        _position = source.CurrentPosition;
        Extract();
    }

    // ---------------------------------------------------------------------
    // Methods
    // ---------------------------------------------------------------------

    /// <summary>
    /// Extracts the token's text and value from the source. Default behavior captures
    /// the current character and advances the source by one character.
    /// </summary>
    protected virtual void Extract()
    {
        _text = CurrentCharacter.ToString();
        _value = null;
        _ = GetNextCharacter();
    }

    private char GetNextCharacter() => _source.GetNextCharacter();

    /// <summary>
    /// Peeks at the next character in the source without consuming it.
    /// </summary>
    protected char PeekCharacter() => _source.PeekCharacter();
}
