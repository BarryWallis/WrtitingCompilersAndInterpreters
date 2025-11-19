using MessageLib;

namespace Chapter2;

/// <summary>
/// Listens for parser summary messages and writes parsing metrics to the console.
/// </summary>
internal class ParserMessageListener : IMessageListener
{
    /// <summary>
    /// Receives a dispatched message and processes known parser-specific message types.
    /// </summary>
    /// <param name="message">The message instance dispatched by the parser.</param>
    public void MessageReceived(Message message)
    {
        switch (message)
        {
            case ParserSummaryMessage parserSummaryMessage:
                ConsoleMetricsWriter.WriteMetric(parserSummaryMessage.LineCount, "source lines.", leadingBlankLine: true);
                ConsoleMetricsWriter.WriteMetric(parserSummaryMessage.ErrorCount, "syntax errors.");
                ConsoleMetricsWriter.WriteSeconds(parserSummaryMessage.ElapsedTime.TotalSeconds, "seconds total parsing time.");
                break;
            default:
                // Fail fast if a new message type was introduced but not handled.
                message.UnknownMessageType();
                break;
        }
    }
}
