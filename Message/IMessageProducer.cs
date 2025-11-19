namespace MessageLib;

/// <summary>
/// Defines an object capable of producing <see cref="Message"/> instances to listeners.
/// </summary>
public interface IMessageProducer
{
    /// <summary>
    /// Subscribes a listener to receive messages.
    /// </summary>
    void AddMessageListener(IMessageListener listener);

    /// <summary>
    /// Unsubscribes a listener from receiving messages.
    /// </summary>
    void RemoveMessageListener(IMessageListener listener);

    /// <summary>
    /// Sends the specified message to all listeners.
    /// </summary>
    void SendMessage(Message message);
}
