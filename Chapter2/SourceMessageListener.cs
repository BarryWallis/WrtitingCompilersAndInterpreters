using MessageLib;

namespace Chapter2;

/// <summary>
/// Listens for <see cref="SourceLineMessage"/> instances and writes each source line
/// to the console, prefixed by its line number.
/// </summary>
internal class SourceMessageListener : IMessageListener
{
    public SourceMessageListener()
    {
    }

    /// <summary>
    /// Handles incoming source-related messages.
    /// </summary>
    /// <param name="message">The dispatched message.</param>
    public void MessageReceived(Message message)
    {
        switch (message)
        {
            case SourceLineMessage(int lineNumber, string line):
                // Echo each source line for tracing/debugging purposes.
                Console.WriteLine($"{lineNumber}: {line}");
                break;
            default:
                // Fail fast if a new message type was introduced but not handled.
                message.UnknownMessageType();
                break;
        }
    }
}
