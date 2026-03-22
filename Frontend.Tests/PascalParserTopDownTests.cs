using System.Collections.Generic;
using System.IO;
using System.Text;

using Frontend.PascalLib;
using FrontendLib;

using MessageLib;

using Xunit;

namespace Frontend.Tests;

public sealed class PascalParserTopDownTests
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
    /// Verifies Parse consumes tokens until EOF and publishes token and parser summary messages.
    /// </summary>
    [Fact]
    public void ParseConsumesTokensUntilEofAndSendsParserSummaryMessage()
    {
        StreamReader reader = CreateReader("input");
        Source source = new(reader);
        TestToken regularToken = new(source, 2);
        EofToken eofToken = new(source)
        {
            LineNumber = 7,
        };
        Queue<Token> tokens = new([regularToken, eofToken]);
        TestScanner scanner = new(source, tokens);
        PascalParserTopDown parser = new(scanner);
        RecordingMessageListener listener = new();

        parser.AddMessageListener(listener);
        parser.Parse();

        Assert.Equal(2, scanner.ExtractCount);
        Assert.Equal(2, listener.Messages.Count);
        TokenMessage tokenMessage = Assert.IsType<TokenMessage>(listener.Messages[0]);
        Assert.Equal(2, tokenMessage.LineNumber);
        ParserSummaryMessage summary = Assert.IsType<ParserSummaryMessage>(listener.Messages[1]);
        Assert.Equal(7, summary.SourceLinesRead);
        Assert.Equal(0, summary.SyntaxErrors);
        Assert.True(summary.ElapsedTime >= 0f);
    }

    /// <summary>
    /// Scanner test double that returns predefined tokens in sequence.
    /// </summary>
    private sealed class TestScanner(Source source, Queue<Token> tokens) : Scanner(source)
    {
        private readonly Queue<Token> _tokens = tokens;

        /// <summary>
        /// Gets the number of extracted tokens.
        /// </summary>
        public int ExtractCount { get; private set; }

        /// <summary>
        /// Returns the next predefined token.
        /// </summary>
        /// <returns>The next token in the queue.</returns>
        protected override Token ExtractToken()
        {
            ExtractCount++;
            return _tokens.Dequeue();
        }
    }

    /// <summary>
    /// Token test double that avoids consuming source characters.
    /// </summary>
    private sealed record TestToken : Token
    {
        /// <summary>
        /// Initializes a token with a specific line number.
        /// </summary>
        /// <param name="source">The source for token construction.</param>
        /// <param name="lineNumber">The line number to assign.</param>
        public TestToken(Source source, int lineNumber) : base(source) => LineNumber = lineNumber;

        /// <summary>
        /// Leaves token extraction empty so tests can control token metadata.
        /// </summary>
        protected override void Extract()
        {
        }
    }

    /// <summary>
    /// Message listener test double that records all received messages.
    /// </summary>
    private sealed class RecordingMessageListener : IMessageListener
    {
        /// <summary>
        /// Gets the received messages in delivery order.
        /// </summary>
        public List<Message> Messages { get; } = [];

        /// <summary>
        /// Records each received message.
        /// </summary>
        /// <param name="message">The received message.</param>
        public void MessageReceived(Message message) => Messages.Add(message);
    }
}
