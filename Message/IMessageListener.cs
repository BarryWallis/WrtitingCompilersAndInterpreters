namespace MessageLib;

/// <summary>
/// Defines a contract for components that listen for and handle incoming messages.
/// </summary>
/// <example>
/// Implement this interface to receive messages from a message source:
/// <code>
/// public class ConsoleMessageListener : IMessageListener
/// {
///     public void MessageReceived(MessageRecord message)
///     {
///         Console.WriteLine($"Received: {message}");
///     }
/// }
/// </code>
/// </example>
public interface IMessageListener
{
    /// <summary>
    /// Called when a new message has been received.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> that was received.</param>
    public void MessageReceived(Message message);
}
