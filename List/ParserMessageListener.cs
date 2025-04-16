using System.Diagnostics;

using Messages;

namespace List;

/// <summary>
/// Handles received messages and processes ParserSummaryMessage to display parsing statistics.
/// </summary>
/// <exception cref="UnreachableException">Message type is unrecognized.</exception>
public class ParserMessageListener : IMessageListener
{
    /// <summary>
    /// Handles incoming messages and processes specific message types for output. It outputs details for
    /// ParserSummaryMessage.
    /// </summary>
    /// <param name="message">Represents the incoming message that is evaluated for processing.</param>
    /// <exception cref="UnreachableException">
    /// Thrown when the message type does not match any expected messages.
    /// </exception>
    public void MessageReceived(Message message)
    {
        switch (message)
        {
            case ParserSummaryMessage parserSummaryMessage:
                WriteParserSummaryMessage(parserSummaryMessage);
                break;
            default:
                throw new UnreachableException();
        }
    }

    private static void WriteParserSummaryMessage(ParserSummaryMessage parserSummaryMessage)
    {
        Console.WriteLine();
        Console.WriteLine($"{parserSummaryMessage.NumberOfLines,20} source lines.");
        Console.WriteLine($"{parserSummaryMessage.ErrorCount,20} syntax errors.");
        Console.WriteLine($"{parserSummaryMessage.ElapsedTIme,20:F2} seconds total parsing time.");
        Console.WriteLine();
    }
}