using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend;

/// <summary>
/// Handles reading characters from the source program line by line, tracking the current position and line 
/// number.
/// </summary>
/// <param name="reader">Provides the input stream for reading the source program.</param>
public class Source(StreamReader reader)
{
    private string? _line;
    private readonly StreamReader _reader = reader;

    public const char EOF = '\0';
    public const char EOL = '\n';

    public int LineNumber { get; private set; } = 0;
    public int? Position { get; private set; } = null;

    /// <summary>
    /// Retrieves the current character from a line based on the current position. 
    /// </summary>
    /// <returns>
    /// The current character, EOF, or EOL based on the state of the line and position.
    /// </returns>
    public char GetCurrentChar()
    {
        if (Position is null)
        {
            ReadLine();
            return GetNextChar();
        }
        else if (_line is null)
        {
            return EOF;
        }
        else if (Position == -1 || Position == _line.Length)
        {
            return EOL;
        }
        else if (Position > _line.Length)
        {
            ReadLine();
            return GetNextChar();
        }
        else
        {
            return _line[Position.Value];
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
        Debug.Assert(Position >= -1);

        Position += 1;

        char currentChar = GetCurrentChar();

        Debug.Assert(currentChar == EOF || _line is not null);
        Debug.Assert(currentChar == EOF || (Position <= _line!.Length && Position >= 0));
        return currentChar;
    }

    /// <summary>
    /// Reads a line from the input reader and updates the current position. Increments the line number if 
    /// the read line is not null.
    /// </summary>
    private void ReadLine()
    {
        _line = _reader.ReadLine();
        Position = -1;
        LineNumber += _line is not null ? 1 : 0;
    }

    /// <summary>
    /// Retrieves the character that follows the current position in a line. If the current position is at 
    /// the end of the line, it returns EOL. It does not increment the current position.
    /// </summary>
    /// <returns>The next character or EOF if the line is null.</returns>
    public char PeekChar()
    {
        Debug.Assert(Position is not null);

        _ = GetCurrentChar();
        if (_line is null)
        {
            return EOF;
        }

        int nextPosition = Position.Value + 1;
        return nextPosition < _line.Length ? _line[nextPosition] : EOL;
    }
}
