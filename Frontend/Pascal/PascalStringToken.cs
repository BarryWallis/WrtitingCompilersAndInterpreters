using System.Text;

namespace FrontendComponents.Pascal;

/// <summary>
/// A Pascal token representing a string.
/// </summary>
public record PascalStringToken : PascalToken
{
    /// <summary>
    /// Create a new Pascal string token from the source. 
    /// </summary>
    /// <param name="source"><inheritdoc/></param>
    public PascalStringToken(Source source) : base(source)
    {
    }

    /// <summary>
    /// Extracts the string from the source. The string is defined as a sequence of characters enclosed in 
    /// single quotes.
    /// </summary>
    protected override void Extract()
    {
        StringBuilder textBuffer = new();
        StringBuilder valueBuffer = new();
        char currentChar = GetNextChar();
        _ = textBuffer.Append('\'');
        do
        {
            currentChar = ProcessString(textBuffer, valueBuffer, currentChar);
        } while (currentChar is not '\'' and not Source.EOF);

        if (currentChar == '\'')
        {
            _ = GetNextChar();
            _ = textBuffer.Append('\'');
            Kind = CommonInterfaces.ITokenType.Kind.String;
            Value = valueBuffer.ToString();
        }
        else
        {
            Kind = CommonInterfaces.ITokenType.Kind.Error;
            Value = PascalErrorCode.UnexpectedEof;
        }

        Text = textBuffer.ToString();
    }

    /// <summary>
    /// Processes the string by appending characters to the text and value buffers. It handles double 
    /// single quotes as a single quote in the string.
    /// </summary>
    /// <param name="textBuffer">The buffer to accumulate the literal text.</param>
    /// <param name="valueBuffer">The buffer to accumulate the value of the string.</param>
    /// <param name="currentChar">The current character being processed.</param>
    /// <returns></returns>
    private char ProcessString(StringBuilder textBuffer, StringBuilder valueBuffer, char currentChar)
    {
        if (char.IsWhiteSpace(currentChar))
        {
            currentChar = ' ';
        }

        if (currentChar is not '\'' and not Source.EOF)
        {
            _ = textBuffer.Append(currentChar);
            _ = valueBuffer.Append(currentChar);
            currentChar = GetNextChar();
        }

        if (currentChar == '\'')
        {
            while (currentChar == '\'' && PeekChar() == '\'')
            {
                _ = textBuffer.Append("''");
                _ = valueBuffer.Append(currentChar);
                _ = GetNextChar();
                currentChar = GetNextChar();
            }
        }

        return currentChar;
    }
}