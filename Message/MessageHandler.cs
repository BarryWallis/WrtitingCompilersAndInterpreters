using System.Diagnostics;

namespace MessageLib;

/// <summary>
/// Manages message listeners and dispatches messages to them.
/// </summary>
public class MessageHandler
{
    // ---------------------------------------------------------------------
    // Fields
    // ---------------------------------------------------------------------

    private Message? _message = null;
    private readonly List<IMessageListener> _listeners = [];

    // ---------------------------------------------------------------------
    // Public methods
    // ---------------------------------------------------------------------

    /// <summary>
    /// Registers a listener to receive messages.
    /// </summary>
    public void AddListener(IMessageListener listener) => _listeners.Add(listener);

    /// <summary>
    /// Unregisters a listener from receiving messages.
    /// </summary>
    public void RemoveListener(IMessageListener listener) => _listeners.Remove(listener);

    /// <summary>
    /// Sends a message to all registered listeners.
    /// </summary>
    public void SendMessage(Message message)
    {
        _message = message;
        NotifyListeners();
    }

    // ---------------------------------------------------------------------
    // Private methods
    // ---------------------------------------------------------------------

    /// <summary>
    /// Notifies all registered listeners of the last message.
    /// </summary>
    private void NotifyListeners()
    {
        foreach (IMessageListener listener in _listeners)
        {
            listener.MessageReceived(_message ?? throw new UnreachableException());
        }
    }
}
