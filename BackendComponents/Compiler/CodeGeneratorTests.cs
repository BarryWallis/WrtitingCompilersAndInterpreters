using Intermediate;

using Messages;

using Moq;

using Xunit;

namespace BackendComponents.Compiler;

public class CodeGeneratorTests
{
    [Fact]
    public void Process_ShouldSendCompilerSummaryMessage()
    {
        // Arrange
        Mock<IIntermediateCode> mockIntermediateCode = new();
        Mock<ISymbolTable> mockSymbolTable = new();
        Mock<IMessageListener> mockListener = new();
        CodeGenerator codeGenerator = new();
        codeGenerator.AddMessageListener(mockListener.Object);

        // Act
        codeGenerator.Process(mockIntermediateCode.Object, mockSymbolTable.Object);

        // Assert
        mockListener.Verify(listener => listener.MessageReceived(It.IsAny<CompilerSummaryMessage>()), Times.Once);
    }

    [Fact]
    public void AddMessageListener_ShouldRegisterListener()
    {
        // Arrange
        Mock<IMessageListener> mockListener = new();
        CodeGenerator codeGenerator = new();

        // Act
        codeGenerator.AddMessageListener(mockListener.Object);

        // Assert
        // No exception should be thrown
    }

    [Fact]
    public void RemoveMessageListener_ShouldUnregisterListener()
    {
        // Arrange
        Mock<IMessageListener> mockListener = new();
        CodeGenerator codeGenerator = new();
        codeGenerator.AddMessageListener(mockListener.Object);

        // Act
        codeGenerator.RemoveMessageListener(mockListener.Object);

        // Assert
        // No exception should be thrown
    }

    [Fact]
    public void SendMessage_ShouldNotifyListeners()
    {
        // Arrange
        Mock<IMessageListener> mockListener = new();
        CodeGenerator codeGenerator = new();
        codeGenerator.AddMessageListener(mockListener.Object);
        int executionCount = 0;
        int runtimeErrors = 0;
        double elapsedTime = 0.0;
        InterpreterSummaryMessage message = new(executionCount, runtimeErrors, elapsedTime);

        // Act
        codeGenerator.SendMessage(message);

        // Assert
        mockListener.Verify(listener => listener.MessageReceived(message), Times.Once);
    }
}