using MessageLib;

using Xunit;

namespace Message.Tests;

public sealed class MessageHandlerTests
{
    /// <summary>
    /// Verifies SendMessage notifies a registered listener.
    /// </summary>
    [Fact]
    public void SendMessageNotifiesRegisteredListener()
    {
        MessageHandler handler = new();
        RecordingMessageListener listener = new();
        SourceLineMessage message = new(1, "line");

        handler.AddListener(listener);
        handler.SendMessage(message);

        Assert.Equal(1, listener.ReceivedCount);
        Assert.Same(message, listener.LastMessage);
    }

    /// <summary>
    /// Verifies RemoveListener prevents further notifications to that listener.
    /// </summary>
    [Fact]
    public void RemoveListenerPreventsFurtherNotifications()
    {
        MessageHandler handler = new();
        RecordingMessageListener listener = new();
        SourceLineMessage message = new(2, "line");

        handler.AddListener(listener);
        handler.RemoveListener(listener);
        handler.SendMessage(message);

        Assert.Equal(0, listener.ReceivedCount);
        Assert.Null(listener.LastMessage);
    }

    /// <summary>
    /// Verifies SendMessage notifies all registered listeners.
    /// </summary>
    [Fact]
    public void SendMessageNotifiesAllRegisteredListeners()
    {
        MessageHandler handler = new();
        RecordingMessageListener first = new();
        RecordingMessageListener second = new();
        SourceLineMessage message = new(3, "line");

        handler.AddListener(first);
        handler.AddListener(second);
        handler.SendMessage(message);

        Assert.Equal(1, first.ReceivedCount);
        Assert.Equal(1, second.ReceivedCount);
        Assert.Same(message, first.LastMessage);
        Assert.Same(message, second.LastMessage);
    }

    /// <summary>
    /// Records delivered messages for verification.
    /// </summary>
    private sealed class RecordingMessageListener : IMessageListener
    {
        /// <summary>
        /// Gets the number of received messages.
        /// </summary>
        public int ReceivedCount { get; private set; }

        /// <summary>
        /// Gets the most recently received message.
        /// </summary>
        public MessageLib.Message? LastMessage { get; private set; }

        /// <summary>
        /// Stores the received message and increments the received count.
        /// </summary>
        /// <param name="message">The received message.</param>
        public void MessageReceived(MessageLib.Message message)
        {
            ReceivedCount++;
            LastMessage = message;
        }
    }
}
