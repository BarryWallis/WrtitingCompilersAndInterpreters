using CommonInterfaces;

namespace FrontendComponents.Pascal.Tokens;

/// <summary>
/// A class representing a special symbol token in Pascal.
/// </summary>
public record PascalSpecialSymbolToken : PascalToken
{
    /// <summary>
    /// Create a new SpecialSymbolToken using the provided source. The token type is determined by the source 
    /// data.
    /// </summary>
    /// <param name="source">The source program.</param>
    public PascalSpecialSymbolToken(Source source) : base(source)
    {
    }

    /// <summary>
    /// Extract a SpecialSymbolToken from the source program. The token type is determined by the source 
    /// data.
    /// </summary>
    protected override void Extract()
    {
        char currentChar = CurrentChar();
        Text = currentChar.ToString();
        Kind = null;
        switch (currentChar)
        {
            case '+':
            case '-':
            case '*':
            case '/':
            case ',':
            case (char)59:
            case '\'':
            case '=':
            case '(':
            case ')':
            case '[':
            case ']':
            case '{':
            case '}':
            case '^':
                NextChar();
                break;
            case ':':
                StartsWithColon();
                break;
            case '<':
                StartsWithLessThan();
                break;
            case '>':
                StartsWithGreaterThan();
                break;
            case '.':
                StartsWithDot();
                break;
            default:
                NoSuchSpecialSymbol();
                break;
        }

        Kind ??= PascalTokenType.SpecialSymbols[Text];
    }

    /// <summary>
    /// The current character is not a valid special symbol. 
    /// </summary>
    private void NoSuchSpecialSymbol()
    {
        NextChar();
        Kind = ITokenType.Kind.Error;
        Value = PascalErrorCode.InvalidCharacter;
    }

    /// <summary>
    /// The special symbol starts with a dot. If the next character is also a dot, it is a ".." symbol.
    /// </summary>
    /// <returns></returns>
    private void StartsWithDot()
    {
        char currentChar = GetNextChar();
        if (currentChar == '.')
        {
            Text += currentChar;
            NextChar();
        }
    }

    /// <summary>
    /// The special symbol starts with a greater than sign. If the next character is also an equal sign, it 
    /// is a ">=" symbol.
    /// </summary>
    /// <returns>The character after the special symbol.</returns>
    private void StartsWithGreaterThan()
    {
        char currentChar = GetNextChar();
        if (currentChar == '=')
        {
            Text += currentChar;
            NextChar();
        }
    }

    /// <summary>
    /// The special symbol starts with a less than sign. If the next character is also an equal sign, it is a 
    /// "<=" symbol.
    /// </summary>
    /// <returns>The character after the special symbol.</returns>
    private void StartsWithLessThan()
    {
        char currentChar = GetNextChar();
        if (currentChar == '=')
        {
            Text += currentChar;
            NextChar();
        }
        else if (currentChar == '>')
        {
            Text += currentChar;
            NextChar();
        }
    }

    /// <summary>
    /// The special symbol starts with a colon. If the next character is also an equal sign, it is a ":=" 
    /// symbol.
    /// </summary>
    /// <returns>The character after the special symbol.</returns>
    private void StartsWithColon()
    {
        char currentChar = GetNextChar();
        if (currentChar == '=')
        {
            Text += currentChar;
            NextChar();
        }
    }
}
