namespace FrontendComponents.Pascal;

/// <summary>
/// Extracts a token based on the current character, returning an end-of-file token if the character is 
/// EOF.
/// </summary>
/// <param name="source">
/// rovides the context needed to create tokens based on the current character.
/// </param>
public class PascalScanner(Source source) : Scanner(source)
{
    /// <summary>
    /// Extracts a token based on the current character. It returns an <see cref="EofToken"/> if the 
    /// current character is EOF and an <see cref="PascalErrorToken"/> if there is an error.
    /// </summary>
    /// <returns>Returns correct token depending on the current character.</returns>
    protected override Token ExtractToken()
    {
        SkipWhiteSpace();

        Token token;
        char currentChar = CurrentChar;

        if (currentChar == Source.EOF)
        {
            token = new EofToken(_source);
        }
        else if (char.IsLetter(currentChar))
        {
            token = new PascalWordToken(_source);
        }
        else if (char.IsDigit(currentChar))
        {
            token = new PascalNumberToken(_source);
        }
        else if (currentChar == '\'')
        {
            token = new PascalStringToken(_source);
        }
        else if (currentChar == (char)59)
        {
            token = new PascalSpecialSymbolToken(_source);
        }
        else if (PascalTokenType.SpecialSymbols.ContainsKey(currentChar.ToString()))
        {
            token = new PascalSpecialSymbolToken(_source);
        }
        else
        {
            token = new PascalErrorToken(_source, PascalErrorCode.InvalidCharacter,
                                         currentChar.ToString());
            _ = NextChar;
        }

        return token;
    }

    /// <summary>
    /// Skip whitespace characters by consuming them.  A comment is whitespace.
    /// </summary>
    private void SkipWhiteSpace()
    {
        char currentChar = CurrentChar;

        while (char.IsWhiteSpace(currentChar) || (currentChar == '{'))
        {
            if (currentChar == '{')
            {
                do
                {
                    currentChar = NextChar;
                } while (currentChar is not '}' and not Source.EOF);
                if (currentChar == '}')
                {
                    currentChar = NextChar;
                }
            }
            else
            {
                currentChar = NextChar;
            }
        }
    }
}
