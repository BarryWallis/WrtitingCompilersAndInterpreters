namespace MessageLib;

/// <summary>
/// Defines a listener that can receive messages from an <see cref="IMessageProducer"/>.
/// </summary>
public interface IMessageListener
{
    /// <summary>
    /// Called when a new message is sent by a producer.
    /// </summary>
    void MessageReceived(Message message);
}
