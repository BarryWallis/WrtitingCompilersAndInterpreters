using Xunit;

namespace FrontendComponents.Tests;

public class EofTokenTests : IDisposable
{
    private readonly Source _source;
    private readonly StreamReader _streamReader;

    public EofTokenTests()
    {
        // Initialize the source with an empty input to simulate EOF
        string input = string.Empty;
        _streamReader = new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)));
        _source = new Source(_streamReader);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _source.Close();
        _streamReader.Dispose();
    }

    [Fact]
    public void Constructor_ShouldInitializeEofToken()
    {
        // Arrange & Act
        EofToken eofToken = new(_source);

        // Assert
        Assert.NotNull(eofToken);
        Assert.Equal(0, eofToken.LineNumber); // Line number should be 0 at EOF
        Assert.Null(eofToken.Text); // Text should be null for EOF
        Assert.Null(eofToken.Value); // Value should be null for EOF
    }

    [Fact]
    public void CurrentChar_ShouldReturnEOF()
    {
        // Arrange
        _ = new EofToken(_source);

        // Act
        char currentChar = _source.GetCurrentChar();

        // Assert
        Assert.Equal(Source.EOF, currentChar); // Current character should be EOF
    }
}
