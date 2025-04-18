namespace Messages;

public record SyntaxErrorMessage(int LineNumber, int Position, string? TokenText, string ErrorMessage) 
    : Message
{
}
