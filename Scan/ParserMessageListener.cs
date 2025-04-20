using System.Diagnostics;
using System.Text;

using Messages;

namespace Scan;

/// <summary>
/// Handles received messages and processes ParserSummaryMessage to display parsing statistics.
/// </summary>
/// <exception cref="UnreachableException">Message type is unrecognized.</exception>
public class ParserMessageListener : IMessageListener
{
    const int _prefixWidth = 5;

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
            case TokenMessage tokenMessage:
                WriteTokenMessage(tokenMessage);
                break;
            case SyntaxErrorMessage syntaxErrorMessage:
                WriteSyntaxErrorMessage(syntaxErrorMessage);
                break;
            default:
                throw new UnreachableException();
        }
    }

    private static void WriteSyntaxErrorMessage(SyntaxErrorMessage syntaxErrorMessage)
    {
        int spaceCount = _prefixWidth + syntaxErrorMessage.Position;
        StringBuilder flagBuffer = new();
        _ = flagBuffer.Append(' ', spaceCount).Append('^').AppendLine();
        _ = flagBuffer.Append("*** ").Append(syntaxErrorMessage.ErrorMessage);
        if (syntaxErrorMessage.TokenText is not null)
        {
            _ = flagBuffer.Append(" [at \"").Append(syntaxErrorMessage.TokenText).Append("\"]");
        }

        Console.WriteLine(flagBuffer.ToString());
    }

    private static void WriteTokenMessage(TokenMessage tokenMessage)
    {
        Console.WriteLine($">>> {tokenMessage.Kind,-15} " +
            $"line={tokenMessage.LineNumber:000}, pos={tokenMessage.Position,2}, " +
            $"text=\"{tokenMessage.Text}\"");
        if (tokenMessage.Value is not null)
        {
            if (tokenMessage.Kind.ToString() == "String")
            {
                Debug.Assert(tokenMessage.Value is string);
                tokenMessage = tokenMessage with { Value = $"\"{tokenMessage.Value}\"" };
            }

            Console.WriteLine($">>>                 value={tokenMessage.Value}");
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