namespace Messages;

/// <summary>
/// Represents a summary of parsing results.
/// </summary>
/// <param name="NumberOfLines">Indicates the total number of lines parsed.</param>
/// <param name="ErrorCount">Represents the total number of errors encountered during parsing.</param>
/// <param name="ElapsedTIme">Denotes the total time taken for the parsing process in seconds.</param>
public record ParserSummaryMessage(int NumberOfLines, int ErrorCount, double ElapsedTIme) : Message
{
}
