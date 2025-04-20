namespace Messages;

/// <summary>
/// Represents a message containing information about a syntax error in a source file.
/// </summary>
/// <param name="LineNumber">The line number of the syntax error.</param>
/// <param name="Position">The position of the syntax error.</param>
/// <param name="TokenText">The token's text.</param>
/// <param name="ErrorMessage">The error message</param>
public record SyntaxErrorMessage(int LineNumber, int Position, string? TokenText, string ErrorMessage) 
    : Message
{
}
