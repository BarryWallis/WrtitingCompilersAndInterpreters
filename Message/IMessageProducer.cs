namespace Messages;

/// <summary>
/// IMessageProducer allows for the registration and removal of message listeners. It also facilitates 
/// sending messages to recipients.
/// </summary>
public interface IMessageProducer
{
    /// <summary>
    /// Registers a listener to receive message notifications.
    /// </summary>
    /// <param name="listener">The provided listener will handle incoming messages.</param>
    void AddMessageListener(IMessageListener listener);

    /// <summary>
    /// Removes a message listener from the system, stopping it from receiving further messages.
    /// </summary>
    /// <param name="listener">
    /// The listener to be removed, which previously handled incoming messages.
    /// </param>
    void RemoveMessageListener(IMessageListener listener);

    /// <summary>
    /// Sends a message to a specified recipient or system. This action facilitates communication between 
    /// components.
    /// </summary>
    /// <param name="message">The content and details of the communication being sent.</param>
    void SendMessage(Message message);
}
