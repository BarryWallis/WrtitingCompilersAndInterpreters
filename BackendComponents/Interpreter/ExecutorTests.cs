using Intermediate;

using Messages;

using Moq;

using Xunit;

namespace BackendComponents.Interpreter;

public class ExecutorTests
{
    [Fact]
    public void Process_ShouldSendInterpreterSummaryMessage()
    {
        // Arrange
        Mock<IIntermediateCode> mockIntermediateCode = new();
        Mock<ISymbolTableStack> mockSymbolTableStack = new();
        Mock<IMessageListener> mockListener = new();
        Executor executor = new();
        executor.AddMessageListener(mockListener.Object);

        // Act
        executor.Process(mockIntermediateCode.Object, mockSymbolTableStack.Object);

        // Assert
        mockListener.Verify(listener => listener.MessageReceived(It.IsAny<InterpreterSummaryMessage>()), Times.Once);
    }

    [Fact]
    public void AddMessageListener_ShouldRegisterListener()
    {
        // Arrange
        Mock<IMessageListener> mockListener = new();
        Executor executor = new();

        // Act
        executor.AddMessageListener(mockListener.Object);

        // Assert
        // No exception should be thrown
    }

    [Fact]
    public void RemoveMessageListener_ShouldUnregisterListener()
    {
        // Arrange
        Mock<IMessageListener> mockListener = new();
        Executor executor = new();
        executor.AddMessageListener(mockListener.Object);

        // Act
        executor.RemoveMessageListener(mockListener.Object);

        // Assert
        // No exception should be thrown
    }

    [Fact]
    public void SendMessage_ShouldNotifyListeners()
    {
        // Arrange
        Mock<IMessageListener> mockListener = new();
        Executor executor = new();
        executor.AddMessageListener(mockListener.Object);
        int instructionCount = 0;
        double elapsedTime = 0.0;
        CompilerSummaryMessage message = new(instructionCount, elapsedTime);

        // Act
        executor.SendMessage(message);

        // Assert
        mockListener.Verify(listener => listener.MessageReceived(message), Times.Once);
    }
}
