using System.Text;

using Messages;

using Moq;

using Xunit;

namespace FrontendComponents.Tests;

public class SourceTests : IDisposable
{
    private readonly Source _source;
    private readonly StringReader _stringReader;

    public SourceTests()
    {
        // Sample input for testing
        string input = "Line1\nLine2\nLine3";
        _stringReader = new StringReader(input);

        byte[] byteArray = Encoding.UTF8.GetBytes(input);
        MemoryStream memoryStream = new(byteArray);
        StreamReader streamReader = new(memoryStream);
        _source = new Source(streamReader);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _source.Close();
        _stringReader.Dispose();
    }

    [Fact]
    public void GetCurrentChar_ShouldReturnFirstCharacter()
    {
        char result = _source.GetCurrentChar();
        Assert.Equal('L', result); // The first character should be 'L'
    }

    [Fact]
    public void GetNextChar_ShouldReturnNextCharacter()
    {
        _ = _source.GetCurrentChar(); // Move to the first character
        char result = _source.GetNextChar();
        Assert.Equal('i', result); // The next character should be 'i'
    }

    [Fact]
    public void GetCurrentChar_ShouldReturnEOLAtEndOfLine()
    {
        // Move to the end of the first line
        while (_source.GetCurrentChar() != Source.EOL)
        { _ = _source.GetNextChar(); }

        char result = _source.GetCurrentChar();
        Assert.Equal(Source.EOL, result); // The character at the end of the line should be EOL
    }

    [Fact]
    public void GetCurrentChar_ShouldReturnEOFAtEndOfFile()
    {
        // Read all characters until EOF
        while (_source.GetCurrentChar() != Source.EOF)
        { _ = _source.GetNextChar(); }

        char result = _source.GetCurrentChar();
        Assert.Equal(Source.EOF, result); // The character at the end of the file should be EOF
    }

    [Fact]
    public void PeekChar_ShouldReturnNextCharacterWithoutAdvancingPosition()
    {
        char current = _source.GetCurrentChar();
        char peeked = _source.PeekChar();

        Assert.Equal('i', peeked); // PeekChar should return the next character without advancing
        Assert.Equal(current, _source.GetCurrentChar()); // GetCurrentChar should not be affected by PeekChar
    }

    [Fact]
    public void Close_ShouldReleaseResources()
    {
        _source.Close();

        _ = Assert.Throws<ObjectDisposedException>(() => _source.GetCurrentChar()); // Accessing after Close should throw an exception
    }

    [Fact]
    public void AddMessageListener_ShouldRegisterListener()
    {
        // Arrange
        Mock<IMessageListener> mockListener = new();

        // Act
        _source.AddMessageListener(mockListener.Object);

        // Assert
        // Simulate sending a message to verify the listener is invoked
        Mock<Message> testMessage = new();
        _source.SendMessage(testMessage.Object);
        mockListener.Verify(listener
            => listener.MessageReceived(testMessage.Object), Times.Once);
    }

    [Fact]
    public void RemoveMessageListener_ShouldUnregisterListener()
    {
        // Arrange
        Mock<IMessageListener> mockListener = new();
        _source.AddMessageListener(mockListener.Object);

        // Act
        _source.RemoveMessageListener(mockListener.Object);

        // Assert
        // Simulate sending a message to verify the listener is not invoked
        Mock<Message> testMessage = new();
        _source.SendMessage(testMessage.Object);
        mockListener.Verify(listener => listener.MessageReceived(It.IsAny<Message>()), Times.Never);
    }

    [Fact]
    public void SendMessage_ShouldNotifyAllListeners()
    {
        // Arrange
        Mock<IMessageListener> mockListener1 = new();
        Mock<IMessageListener> mockListener2 = new();
        _source.AddMessageListener(mockListener1.Object);
        _source.AddMessageListener(mockListener2.Object);

        // Act
        Mock<Message> testMessage = new();
        _source.SendMessage(testMessage.Object);

        // Assert
        mockListener1.Verify(listener => listener.MessageReceived(testMessage.Object), Times.Once);
        mockListener2.Verify(listener => listener.MessageReceived(testMessage.Object), Times.Once);
    }
}
