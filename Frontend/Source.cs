using System.Diagnostics;
using MessageLib;

namespace FrontendLib;

/// <summary>
/// Reads characters from a <see cref="StreamReader"/> one line at a time and exposes
/// a character-by-character interface with lookahead support. Also publishes source line
/// messages to registered listeners.
/// </summary>
/// <remarks>
/// This class follows a typical compiler front-end source abstraction, where callers
/// can read the current character, advance to the next, and peek ahead without consuming.
/// It also sends a <see cref="SourceLineMessage"/> for each line read to support
/// diagnostics and tracing.
/// </remarks>
public class Source(StreamReader reader) : IMessageProducer
{
    // ---------------------------------------------------------------------
    // Constants (public to allow consumers to compare sentinel values)
    // ---------------------------------------------------------------------

    /// <summary>
    /// The sentinel character returned at the logical end of each source line.
    /// </summary>
    public const char EndOfLine = '\n';

    /// <summary>
    /// The sentinel character returned when no more input is available.
    /// </summary>
    public const char EndOfFile = '\0';

    // Internal sentinels for current position state
    private const int NoSourceLineRead = -2;
    private const int LineNotRead = -1;

    // ---------------------------------------------------------------------
    // Instance fields
    // ---------------------------------------------------------------------

    private readonly StreamReader _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    private string? _line;
    private readonly MessageHandler _messageHandler = new();

    // ---------------------------------------------------------------------
    // Properties
    // ---------------------------------------------------------------------

    /// <summary>
    /// Gets the current 1-based line number. Remains 0 until the first line is read.
    /// </summary>
    public int LineNumber { get; private set; } = 0;

    /// <summary>
    /// Gets the current character index within the current line, or a sentinel value
    /// when a line has not yet been read.
    /// </summary>
    public int CurrentPosition { get; private set; } = NoSourceLineRead;

    /// <summary>
    /// Gets the current character. This will trigger line loading when necessary.
    /// Returns <see cref="EndOfLine"/> at the end of a line and <see cref="EndOfFile"/>
    /// when no more input is available.
    /// </summary>
    public char CurrentCharacter
    {
        get
        {
            if (CurrentPosition == NoSourceLineRead)
            {
                ReadLine();
                return GetNextCharacter();
            }
            else if (_line is null)
            {
                return EndOfFile;
            }
            else if (CurrentPosition == LineNotRead || CurrentPosition == _line.Length)
            {
                return EndOfLine;
            }
            else if (CurrentPosition > _line.Length)
            {
                ReadLine();
                return GetNextCharacter();
            }
            else
            {
                return _line[CurrentPosition];
            }
        }
    }

    // ---------------------------------------------------------------------
    // Public methods
    // ---------------------------------------------------------------------

    /// <summary>
    /// Advances to the next character and returns it. May trigger reading the next line
    /// when the end of a line is passed.
    /// </summary>
    /// <returns>The next character, or a sentinel when appropriate.</returns>
    public char GetNextCharacter()
    {
        CurrentPosition += 1;
        return CurrentCharacter;
    }

    /// <summary>
    /// Returns the next character without consuming it. Ensures the current line is
    /// loaded before peeking. Returns <see cref="EndOfLine"/> at the end of a line and
    /// <see cref="EndOfFile"/> when no more input is available.
    /// </summary>
    public char PeekCharacter()
    {
        _ = CurrentCharacter; // Ensure the current line/position state is initialized.
        if (_line is null)
        {
            return EndOfFile;
        }

        int nextPosition = CurrentPosition + 1;
        return nextPosition < _line.Length ? _line[nextPosition] : EndOfLine;
    }

    /// <summary>
    /// Closes the underlying <see cref="StreamReader"/>.
    /// </summary>
    public void Close() => _reader.Close();

    /// <summary>
    /// Registers a listener for messages emitted by this source.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public void AddMessageListener(IMessageListener listener) => _messageHandler.AddListener(listener);

    /// <summary>
    /// Unregisters a listener from messages emitted by this source.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public void RemoveMessageListener(IMessageListener listener) => _messageHandler.RemoveListener(listener);

    /// <summary>
    /// Sends a message to all registered listeners.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void SendMessage(Message message) => _messageHandler.SendMessage(message);

    // ---------------------------------------------------------------------
    // Private helpers
    // ---------------------------------------------------------------------

    /// <summary>
    /// Reads the next line from the underlying reader and updates state. Publishes a
    /// <see cref="SourceLineMessage"/> for each successfully read line.
    /// </summary>
    private void ReadLine()
    {
        _line = _reader.ReadLine();
        CurrentPosition = LineNotRead;

        if (_line is not null)
        {
            LineNumber += 1;
            // _line cannot be null in this branch, the guard above ensures that.
            SendMessage(new SourceLineMessage(LineNumber, _line ?? throw new UnreachableException()));
        }
    }
}
