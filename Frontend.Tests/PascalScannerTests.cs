using System.IO;
using System.Text;

using Frontend.PascalLib;
using FrontendLib;

using Xunit;

namespace Frontend.Tests;

public sealed class PascalScannerTests
{
    /// <summary>
    /// Creates a <see cref="StreamReader"/> for test content.
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
    /// Verifies the scanner returns an EOF token when the source is empty.
    /// </summary>
    [Fact]
    public void NextTokenWhenSourceIsEmptyReturnsEofToken()
    {
        StreamReader reader = CreateReader(string.Empty);
        Source source = new(reader);
        PascalScanner scanner = new(source);

        Token token = scanner.NextToken();

        _ = Assert.IsType<EofToken>(token);
    }

    /// <summary>
    /// Verifies the scanner returns a regular token for non-empty input.
    /// </summary>
    [Fact]
    public void NextTokenWhenSourceHasContentReturnsRegularToken()
    {
        StreamReader reader = CreateReader("a");
        Source source = new(reader);
        PascalScanner scanner = new(source);

        Token token = scanner.NextToken();

        Assert.IsNotType<EofToken>(token);
        _ = Assert.IsType<Token>(token);
    }

    /// <summary>
    /// Verifies repeated scanning eventually reaches EOF.
    /// </summary>
    [Fact]
    public void NextTokenAfterInputConsumptionEventuallyReturnsEofToken()
    {
        StreamReader reader = CreateReader("a");
        Source source = new(reader);
        PascalScanner scanner = new(source);

        Token first = scanner.NextToken();
        Token second = scanner.NextToken();
        Token third = scanner.NextToken();

        Assert.IsNotType<EofToken>(first);
        Assert.IsNotType<EofToken>(second);
        _ = Assert.IsType<EofToken>(third);
    }
}
