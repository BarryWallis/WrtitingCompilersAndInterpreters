namespace Messages;

public interface IMessageListener
{
    /// <summary>
    /// Handles the event when a message is received. It processes the incoming message for further actions.
    /// </summary>
    /// <param name="message">The incoming data that needs to be processed or acted upon.</param>
    void MessageReceived(Message message);
}