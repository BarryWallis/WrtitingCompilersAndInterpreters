namespace Messages;

/// <summary>
/// Represents a message containing information about a specific line in a source file.
/// </summary>
/// <param name="SourceLineNumber">Indicates the line number in the source file.</param>
/// <param name="Line">Contains the text of the line from the source file.</param>
public record SourceLineMessage(int SourceLineNumber, string Line) : Message
{
}
