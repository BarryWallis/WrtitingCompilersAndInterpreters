using System.Collections.Generic;

namespace MessageLib;

/// <summary>
/// Maintains a collection of message listeners and notifies them when a message is sent.
/// </summary>
public class MessageHandler
{
    private readonly List<IMessageListener> _listeners;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageHandler"/> class.
    /// </summary>
    public MessageHandler() => _listeners = [];

    /// <summary>
    /// Adds a listener to the listener list.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public void AddListener(IMessageListener listener) => _listeners.Add(listener);

    /// <summary>
    /// Removes a listener from the listener list.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public void RemoveListener(IMessageListener listener) => _listeners.Remove(listener);

    /// <summary>
    /// Notifies listeners that a message was sent.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void SendMessage(Message message) => NotifyListeners(message);

    /// <summary>
    /// Notifies each listener by calling <see cref="IMessageListener.MessageReceived(Message)"/>.
    /// </summary>
    /// <param name="message">The message to deliver.</param>
    private void NotifyListeners(Message message)
    {
        foreach (IMessageListener listener in _listeners)
        {
            listener.MessageReceived(message);
        }
    }
}
