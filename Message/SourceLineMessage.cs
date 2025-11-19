namespace MessageLib;

/// <summary>
/// Message indicating a source line has been read. Contains the 1-based line number and raw line text.
/// </summary>
/// <param name="LineNumber">The 1-based line number.</param>
/// <param name="Line">The raw line text.</param>
public record SourceLineMessage(int LineNumber, string Line) : Message;
