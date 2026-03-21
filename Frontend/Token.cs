namespace FrontendLib;

/// <summary>
/// Represents a token returned by the scanner.
/// </summary>
public record Token
{
    protected string text;
    protected object? value;
    protected Source source;
    protected int position;

    public int LineNumber;

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class.
    /// </summary>
    /// <param name="source">The source from where to fetch token characters.</param>
    public Token(Source theSource)
    {
        source = theSource;
        LineNumber = source.LineNumber;
        position = source.Position;
        text = string.Empty;
        Extract();
    }

    /// <summary>
    /// Extracts a default one-character token from the source.
    /// </summary>
    protected virtual void Extract()
    {
        text = CurrentChar().ToString();
        value = null;
        _ = NextChar();
    }

    /// <summary>
    /// Returns the current character from the source.
    /// </summary>
    /// <returns>The current source character.</returns>
    protected char CurrentChar() => source.CurrentChar();

    /// <summary>
    /// Advances the source and returns the next character.
    /// </summary>
    /// <returns>The next source character.</returns>
    protected char NextChar() => source.NextChar();

    /// <summary>
    /// Returns the next character without consuming it.
    /// </summary>
    /// <returns>The next source character.</returns>
    protected char PeekChar() => source.PeekChar();
}
