using System.IO;
using System.Text;

using FrontendLib;

using Xunit;

namespace Frontend.Tests;

public sealed class TokenTests
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
    /// Verifies constructor extraction captures one-character text and advances the source.
    /// </summary>
    [Fact]
    public void ConstructorExtractsOneCharacterAndAdvancesSource()
    {
        StreamReader reader = CreateReader("ab");
        Source source = new(reader);

        TestToken token = new(source);
        char currentAfterToken = source.CurrentChar();

        Assert.Equal("a", token.Text);
        Assert.Null(token.Value);
        Assert.Equal(0, token.LineNum);
        Assert.Equal(-2, token.Position);
        Assert.Equal('b', currentAfterToken);
    }

    /// <summary>
    /// Verifies constructor snapshots current source line and position before extraction.
    /// </summary>
    [Fact]
    public void ConstructorSnapshotsExistingSourceCoordinates()
    {
        StreamReader reader = CreateReader("ab");
        Source source = new(reader);
        _ = source.CurrentChar();

        TestToken token = new(source);

        Assert.Equal(1, token.LineNum);
        Assert.Equal(0, token.Position);
        Assert.Equal("a", token.Text);
    }

    /// <summary>
    /// Exposes protected token members for test verification.
    /// </summary>
    private sealed record TestToken(Source source) : Token(source)
    {
        /// <summary>
        /// Gets the extracted token text.
        /// </summary>
        public string Text => text;

        /// <summary>
        /// Gets the extracted token value.
        /// </summary>
        public object? Value => value;

        /// <summary>
        /// Gets the captured token line number.
        /// </summary>
        public int LineNum => LineNumber;

        /// <summary>
        /// Gets the captured token source position.
        /// </summary>
        public int Position => position;
    }
}
