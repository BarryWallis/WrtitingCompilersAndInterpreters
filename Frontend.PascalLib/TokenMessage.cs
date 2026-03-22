using MessageLib;

namespace Frontend.PascalLib;

/// <summary>
/// A message that carries the details of a single Pascal token.
/// </summary>
/// <param name="LineNumber">The source line where the token appears.</param>
/// <param name="Position">The character position of the token on the line.</param>
/// <param name="TokenType">The Pascal token classification.</param>
/// <param name="Text">The raw text of the token.</param>
/// <param name="Value">The parsed value of the token, if any.</param>
public record TokenMessage(
    int LineNumber,
    int Position,
    Type TokenType,
    string Text,
    object? Value) : Message;
