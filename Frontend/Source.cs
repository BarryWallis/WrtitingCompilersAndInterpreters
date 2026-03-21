using MessageLib;

namespace FrontendLib;

/// <summary>
/// Represents the source program and provides character-level access.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Source"/> class.
/// </remarks>
/// <param name="reader">The reader for the source program.</param>
public sealed class Source(StreamReader reader) : IDisposable, IMessageProducer
{
    /// <summary>
    /// Gets the end-of-line character.
    /// </summary>
    public const char EOL = '\n';           // end-of-line character

    /// <summary>
    /// Gets the end-of-file character.
    /// </summary>
    public const char EOF = (char)0;        // end-of-file character

    private const int _initialPosition = -2;
    private const int _beforeLineStartPosition = -1;

    private readonly StreamReader _reader = reader;  // reader for the source program
    private string? _line;                  // source line
    private readonly MessageHandler _messageHandler = new();

    /// <summary>
    /// Gets the current source line number.
    /// </summary>
    public int LineNumber { get; private set; }

    /// <summary>
    /// Gets the current source line position.
    /// </summary>
    public int Position { get; private set; } = _initialPosition;

    /// <summary>
    /// Gets the source character at the current position.
    /// </summary>
    /// <returns>The source character at the current position.</returns>
    public char CurrentChar()
    {
        // First time?
        if (Position == _initialPosition)
        {
            ReadLine();
            return NextChar();
        }

        // At end of file?
        if (_line is null)
        {
            return EOF;
        }

        // At end of line?
        if (Position == _beforeLineStartPosition || Position == _line.Length)
        {
            return EOL;
        }

        // Need to read the next line?
        if (Position > _line.Length)
        {
            ReadLine();
            return NextChar();
        }

        // Return the character at the current position.
        return _line[Position];
    }

    /// <summary>
    /// Consumes the current source character and returns the next character.
    /// </summary>
    /// <returns>The next source character.</returns>
    public char NextChar()
    {
        Position++;
        return CurrentChar();
    }

    /// <summary>
    /// Returns the source character following the current character without consuming it.
    /// </summary>
    /// <returns>The following character.</returns>
    public char PeekChar()
    {
        _ = CurrentChar();
        if (_line is null)
        {
            return EOF;
        }

        int nextPos = Position + 1;
        return nextPos < _line.Length ? _line[nextPos] : EOL;
    }

    /// <summary>
    /// Releases the resources used by the <see cref="Source"/>.
    /// </summary>
    public void Dispose()
    {
        _reader.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Reads the next source line.
    /// </summary>
    private void ReadLine()
    {
        _line = _reader.ReadLine();  // null when at the end of the source
        Position = _beforeLineStartPosition;
        if (_line is not null)
        {
            LineNumber++;
            SendMessage(new SourceLineMessage(LineNumber, _line));
       }
    }

    /// <summary>
    /// Adds a message listener to receive source-related messages.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public void AddMessageListener(IMessageListener listener) => _messageHandler.AddListener(listener);

    /// <summary>
    /// Removes a previously registered message listener.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public void RemoveMessageListener(IMessageListener listener) => _messageHandler.RemoveListener(listener);

    /// <summary>
    /// Sends a message to all registered listeners.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void SendMessage(Message message) => _messageHandler.SendMessage(message);
}
