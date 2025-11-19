using FrontendLib;
using FrontendLib.Pascal;

using Xunit;

namespace Tests;

public class ScannerTokenTests
{
    private static Source CreateSource(string text)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
        return new Source(new StreamReader(new MemoryStream(bytes)));
    }

    [Fact]
    public void GetNextToken_ShouldReturnRegularTokenThenEof()
    {
        Source source = CreateSource("A");
        PascalScanner scanner = new(source);
        Token first = scanner.GetNextToken();
        _ = Assert.IsType<Token>(first);  // Assuming 'A' produces a regular token
        Token second = scanner.GetNextToken();
        _ = Assert.IsType<Token>(second);  // Assuming scanner adds a token for EOL
        Token third = scanner.GetNextToken();
        _ = Assert.IsType<EndOfFileToken>(third);
    }

    [Fact]
    public void EndOfFileToken_AfterEmptySource()
    {
        Source source = CreateSource(string.Empty);
        PascalScanner scanner = new(source);
        Token eof = scanner.GetNextToken();
        _ = Assert.IsType<EndOfFileToken>(eof);
    }
}
