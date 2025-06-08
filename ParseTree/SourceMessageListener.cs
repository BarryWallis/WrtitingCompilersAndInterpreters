using System.Diagnostics;

using Messages;

namespace AssignInterpreter;

/// <summary>
/// Handles incoming messages and processes SourceLineMessage types. Outputs the source line number and line 
/// content.
/// </summary>
public class SourceMessageListener : IMessageListener
{
    /// <summary>
    /// Handles incoming messages and processes them based on their type. It specifically formats and 
    /// outputs source line messages.
    /// </summary>
    /// <param name="message">Represents the incoming message that needs to be processed.</param>
    /// <exception cref="UnreachableException">
    /// |Thrown when the message type does not match any expected messages.
    ///</exception>
    public void MessageReceived(Message message)
    {
        switch (message)
        {
            case SourceLineMessage sourceLineMessage:
                Console.WriteLine($"{sourceLineMessage.SourceLineNumber:000} {sourceLineMessage.Line}");
                break;
            default:
                throw new UnreachableException();
        }
    }
}
