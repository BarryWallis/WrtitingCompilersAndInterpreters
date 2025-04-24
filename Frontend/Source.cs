using System.Diagnostics;

using Messages;

namespace FrontendComponents;

/// <summary>
/// Handles reading characters from the source program line by line, tracking the current position and line 
/// number.
/// </summary>
/// <param name="reader">Provides the input stream for reading the source program.</param>
public class Source(StreamReader reader) : IMessageProducer
{
    private string? _line;
    private readonly StreamReader _reader = reader;
    private readonly MessageHandler _messageHandler = new();

    public const char EOF = '\0';
    public const char EOL = '\n';

    public int LineNumber { get; private set; } = 0;
    public int? CurrentPosition { get; private set; } = null;

    /// <summary>
    /// Retrieves the current character from a line based on the current position. 
    /// </summary>
    /// <returns>
    /// The current character, EOF, or EOL based on the state of the line and position.
    /// </returns>
    public char GetCurrentChar()
    {
        if (CurrentPosition is null)
        {
            ReadLine();
            return GetNextChar();
        }
        else if (_line is null)
        {
            return EOF;
        }
        else if (CurrentPosition == -1 || CurrentPosition == _line.Length)
        {
            return EOL;
        }
        else if (CurrentPosition > _line.Length)
        {
            ReadLine();
            return GetNextChar();
        }
        else
        {
            return _line[CurrentPosition.Value];
        }
    }

    /// <summary>
    /// Closes the underlying reader, releasing any resources associated with it.
    /// </summary>
    public void Close() => _reader.Close();

    /// <summary>
    /// Retrieves the next character from the current position in a line. It increments the current position 
    /// before returning the character.
    /// </summary>
    /// <returns>The character at the updated current position.</returns>
    public char GetNextChar()
    {
        Debug.Assert(CurrentPosition >= -1);

        CurrentPosition += 1;

        char currentChar = GetCurrentChar();

        Debug.Assert(currentChar == EOF || _line is not null);
        Debug.Assert(currentChar == EOF || (CurrentPosition <= _line!.Length && CurrentPosition >= 0));
        return currentChar;
    }

    /// <summary>
    /// Reads a line from the input reader and updates the current position. Increments the line number if 
    /// the read line is not null.
    /// </summary>
    private void ReadLine()
    {
        _line = _reader.ReadLine();
        CurrentPosition = -1;
        if (_line is not null)
        {
            LineNumber += 1;
            SendMessage(new SourceLineMessage(LineNumber, _line));
        }
    }

    /// <summary>
    /// Retrieves the character that follows the current position in a line. If the current position is at 
    /// the end of the line, it returns EOL. It does not increment the current position.
    /// </summary>
    /// <returns>The next character or EOF if the line is null.</returns>
    public char PeekChar()
    {
        Debug.Assert(CurrentPosition is not null);

        _ = GetCurrentChar();
        if (_line is null)
        {
            return EOF;
        }

        int nextPosition = CurrentPosition.Value + 1;
        return nextPosition < _line.Length ? _line[nextPosition] : EOL;
    }

    /// <summary>
    /// Adds a listener to handle incoming messages.
    /// </summary>
    /// <param name="listener">The provided listener will be notified when a new message is received.</param>
    public void AddMessageListener(IMessageListener listener) => _messageHandler.AddListener(listener);

    /// <summary>
    /// Removes a message listener from the message handler.
    /// </summary>
    /// <param name="listener">The listener to be removed from the message handling process.</param>
    public void RemoveMessageListener(IMessageListener listener) => _messageHandler.RemoveListener(listener);

    /// <summary>
    /// Sends a message using the message handler.
    /// </summary>
    /// <param name="message">The message to be sent through the message handler.</param>
    public void SendMessage(Message message) => _messageHandler.SendMessage(message);
}
