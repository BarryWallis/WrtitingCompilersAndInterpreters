using System.IO;
using System.Text;

using FrontendLib;

using Xunit;

namespace Frontend.Tests;

public sealed class SourceTests
{
    /// <summary>
    /// Creates a StreamReader for test content.
    /// </summary>
    /// <param name="content">The content to read.</param>
    /// <returns>A reader over the provided content.</returns>
    private static StreamReader CreateReader(string content)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        MemoryStream stream = new(bytes);
        return new StreamReader(stream, Encoding.UTF8, false, 1024, false);
    }

    /// <summary>
    /// Verifies CurrentChar returns the first character in the source.
    /// </summary>
    [Fact]
    public void CurrentCharReturnsFirstCharacter()
    {
        StreamReader reader = CreateReader("abc");
        Source source = new(reader);

        char result = source.CurrentChar();

        Assert.Equal('a', result);
    }

    /// <summary>
    /// Verifies NextChar transitions through end-of-line and end-of-file characters.
    /// </summary>
    [Fact]
    public void NextCharTransitionsThroughEndOfLineAndEndOfFile()
    {
        StreamReader reader = CreateReader("a");
        Source source = new(reader);

        char first = source.CurrentChar();
        char endOfLine = source.NextChar();
        char endOfFile = source.NextChar();

        Assert.Equal('a', first);
        Assert.Equal(Source.EOL, endOfLine);
        Assert.Equal(Source.EOF, endOfFile);
    }

    /// <summary>
    /// Verifies PeekChar does not consume the next character.
    /// </summary>
    [Fact]
    public void PeekCharDoesNotConsumeNextCharacter()
    {
        StreamReader reader = CreateReader("ab");
        Source source = new(reader);

        char current = source.CurrentChar();
        char peeked = source.PeekChar();
        char next = source.NextChar();

        Assert.Equal('a', current);
        Assert.Equal('b', peeked);
        Assert.Equal('b', next);
    }
}
