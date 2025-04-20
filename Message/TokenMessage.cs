using CommonInterfaces;

namespace Messages;

public record TokenMessage(int LineNumber, int Position, ITokenType.Kind Kind, string Text, object? Value)
    : Message
{
}
