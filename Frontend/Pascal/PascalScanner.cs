
using CommonInterfaces;

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
    private readonly Dictionary<string, ITokenType> _pascalSpecialSymbols = [];

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
            token = new EofToken(Source);
        }
        else if (char.IsLetter(currentChar))
        {
            token = new PascalWordToken(Source);
        }
        else if (char.IsDigit(currentChar))
        {
            token = new PascalNumberToken(Source);
        }
        else if (currentChar == '\'')
        {
            token = new PascalStringToken(Source);
        }
        else if (_pascalSpecialSymbols.ContainsKey(char.ToString(currentChar)))
        {
            token = new PascalSpecialSymbolToken(Source);
        }
        else
        {
            token = new PascalErrorToken(Source, PascalErrorCode.InvalidCharacter,
                                         char.ToString(currentChar));
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
