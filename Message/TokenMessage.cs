using CommonInterfaces;

namespace Messages;

public record TokenMessage(int LineNumber, int Position, TokenType TokenType, string Text, object? Value) 
    : Message
{
}
