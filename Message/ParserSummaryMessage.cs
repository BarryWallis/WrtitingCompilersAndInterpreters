namespace MessageLib;

/// <summary>
/// Summary message produced after parsing completes.
/// </summary>
/// <param name="LineCount">Total number of source lines processed.</param>
/// <param name="ErrorCount">Total number of syntax errors detected.</param>
/// <param name="ElapsedTime">Total elapsed parsing time.</param>
public record ParserSummaryMessage(int LineCount, int ErrorCount, TimeSpan ElapsedTime) : Message;
