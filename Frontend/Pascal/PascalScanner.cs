namespace FrontendLib.Pascal;

/// <summary>
/// Scanner implementation for the Pascal language.
/// </summary>
public class PascalScanner(Source source) : Scanner(source)
{
    /// <summary>
    /// Extracts the next token from the Pascal source.
    /// </summary>
    protected override Token ExtractToken()
    {
        Token token;
        char currentChar = CurrentCharacter;

        token = currentChar == Source.EndOfFile ? new EndOfFileToken(_source) : new Token(_source);

        return token;
    }
}
