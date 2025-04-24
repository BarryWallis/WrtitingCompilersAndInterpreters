using System.Text;

using CommonInterfaces;

namespace FrontendComponents.Pascal;

/// <summary>
/// A Pascal token representing a word.
/// </summary>
public record PascalWordToken : PascalToken
{
    /// <summary>
    /// Extract a Pascal word token from the source.
    /// </summary>
    /// <param name="source">The program source file.</param>
    public PascalWordToken(Source source) : base(source)
    {
    }

    /// <summary>
    /// Extracts the word from the source. The word is defined as a sequence of letters and digits.
    /// </summary>
    protected override void Extract()
    {
        StringBuilder textBuffer = new();
        char currentChar = CurrentChar();
        while (char.IsLetterOrDigit(currentChar))
        {
            _ = textBuffer.Append(currentChar);
            currentChar = GetNextChar();
        }

        Text = textBuffer.ToString();
        Kind = PascalTokenType._reservedWords.Contains(Text)
            ? Enum.Parse<ITokenType.Kind>(Text, true)
            : ITokenType.Kind.Identifier;
    }
}