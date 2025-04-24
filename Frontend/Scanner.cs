using System.Diagnostics;

namespace FrontendComponents;

/// <summary>
/// An abstract class for scanning input and extracting tokens from a source.
/// </summary>
/// <param name="source">The input source from which characters and tokens are read.</param>
public abstract class Scanner(Source source)
{
    protected Source _source = source;

    public Token? CurrentToken { get; private set; } = null;
    public char CurrentChar => _source.GetCurrentChar();
    public char NextChar => _source.GetNextChar();

    /// <summary>
    /// Retrieves the next token from the input by extracting it from the current position. Updates the 
    /// current token in the process.
    /// </summary>
    /// <returns>The extracted token.</returns>
    public Token GetNextToken()
    {
        CurrentToken = ExtractToken();

        Debug.Assert(CurrentToken is not null);
        return CurrentToken;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected abstract Token ExtractToken();
}
