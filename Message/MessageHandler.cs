using System.Diagnostics;

namespace Messages;

/// <summary>
/// Manages a collection of message listeners, allowing addition, removal, and notification of listeners when 
/// a message is sent.
/// </summary>
public class MessageHandler
{
    private Message? _message = null;
    private readonly HashSet<IMessageListener> _listeners = [];

    /// <summary>
    /// Adds a listener to a collection if it is not already present. Throws an exception if the listener is 
    /// already in the collection.
    /// </summary>
    /// <param name="listener">The listener to be added to the collection for receiving messages.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to add a listener that already exists in the collection.
    /// </exception>
    public void AddListener(IMessageListener listener)
    {
        if (!_listeners.Add(listener))
        {
            throw new InvalidOperationException("Listener already exists.");
        }
    }

    /// <summary>
    /// Sends a message and notifies all listeners of the new message.
    /// </summary>
    /// <param name="message">The message to be sent to the listeners.</param>
    public void SendMessage(Message message)
    {
        _message = message;
        NotifyListeners();
    }

    /// <summary>
    /// Removes a specified listener from the collection of listeners.
    /// </summary>
    /// <param name="listener">The listener to be removed from the collection.</param>
    public void RemoveListener(IMessageListener listener)
    {
        if (!_listeners.Remove(listener))
        {
            throw new InvalidOperationException("Listener does not exist.");
        }
    }

    private void NotifyListeners()
    {
        Debug.Assert(_message is not null, "Message is null");

        foreach (IMessageListener listener in _listeners)
        {
            listener.MessageReceived(_message);
        }
    }
}
