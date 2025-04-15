using Moq;

using Xunit;

namespace Messages.Tests;

public class MessageHandlerTests
{
    [Fact]
    public void AddListener_ShouldAddListenerSuccessfully()
    {
        // Arrange
        MessageHandler messageHandler = new();
        Mock<IMessageListener> mockListener = new();

        // Act
        messageHandler.AddListener(mockListener.Object);

        // Assert
        // No exception should be thrown, and the listener should be added successfully
    }

    [Fact]
    public void RemoveListener_ShouldRemoveListenerSuccessfully()
    {
        // Arrange
        MessageHandler messageHandler = new();
        Mock<IMessageListener> mockListener = new();
        messageHandler.AddListener(mockListener.Object);

        // Act
        messageHandler.RemoveListener(mockListener.Object);

        // Assert
        // No exception should be thrown, and the listener should be removed successfully
    }

    [Fact]
    public void SendMessage_ShouldNotifyAllListeners()
    {
        // Arrange
        MessageHandler messageHandler = new();
        Mock<IMessageListener> mockListener1 = new();
        Mock<IMessageListener> mockListener2 = new();
        Mock<Message> testMessage = new();

        messageHandler.AddListener(mockListener1.Object);
        messageHandler.AddListener(mockListener2.Object);

        // Act
        messageHandler.SendMessage(testMessage.Object);

        // Assert
        mockListener1.Verify(listener
                             => listener.MessageReceived(testMessage.Object), Times.Once);
        mockListener2.Verify(listener
                             => listener.MessageReceived(testMessage.Object), Times.Once);
    }

    [Fact]
    public void AddListener_ShouldNotAllowDuplicateListeners()
    {
        // Arrange
        MessageHandler messageHandler = new();
        Mock<IMessageListener> mockListener = new();

        // Act
        messageHandler.AddListener(mockListener.Object);

        // Assert
        object value = Assert.Throws<InvalidOperationException>(
                              () => messageHandler.AddListener(mockListener.Object));
    }

    [Fact]
    public void RemoveListener_ShouldThrowIfListenerNotFound()
    {
        // Arrange
        MessageHandler messageHandler = new();
        Mock<IMessageListener> mockListener = new();

        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(
                   () => messageHandler.RemoveListener(mockListener.Object));
    }
}