using MessageLib;

using Xunit;

namespace Tests;

public class MessageHandlerTests
{
    private sealed class TestListener : IMessageListener
    {
        public List<Message> Received { get; } = [];
        public void MessageReceived(Message message) => Received.Add(message);
    }

    [Fact]
    public void AddListener_ShouldAdd()
    {
        MessageHandler handler = new();
        TestListener listener = new();
        handler.AddListener(listener);
        handler.SendMessage(new Message());
        _ = Assert.Single(listener.Received);
    }

    [Fact]
    public void RemoveListener_ShouldRemove()
    {
        MessageHandler handler = new();
        TestListener listener = new();
        handler.AddListener(listener);
        handler.RemoveListener(listener);
        handler.SendMessage(new Message());
        Assert.Empty(listener.Received);
    }

    [Fact]
    public void SendMessage_ShouldNotifyAll()
    {
        MessageHandler handler = new();
        TestListener l1 = new();
        TestListener l2 = new();
        handler.AddListener(l1);
        handler.AddListener(l2);
        ParserSummaryMessage msg = new(1, 0, TimeSpan.Zero);
        handler.SendMessage(msg);
        Assert.Equal(msg, l1.Received.Single());
        Assert.Equal(msg, l2.Received.Single());
    }
}
