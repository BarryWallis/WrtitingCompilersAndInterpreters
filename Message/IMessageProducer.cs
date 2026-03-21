using System;
using System.Collections.Generic;
using System.Text;

namespace MessageLib;

public interface IMessageProducer
{
    /// <summary>
    /// Adds a listener to the listener list.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public void AddMessageListener(IMessageListener listener);

    /// <summary>
    /// Removes a listener from the listener list.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public void RemoveMessageListener(IMessageListener listener);

    /// <summary>
    /// Notifies listeners after setting the message.
    /// </summary>
    /// <param name="message">The message to set.</param>
    public void SendMessage(Message message);
}
