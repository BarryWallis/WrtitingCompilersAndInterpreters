namespace Messages;

/// <summary>
/// Represents a summary of parsing results.
/// </summary>
/// <param name="LineNumber">Indicates the total number of lines parsed.</param>
/// <param name="ErrorCount">Represents the total number of errors encountered during parsing.</param>
/// <param name="TotalSeconds">Denotes the total time taken for the parsing process in seconds.</param>
public record ParserSummaryMessage(int LineNumber, int ErrorCount, double TotalSeconds) : Message
{
}
