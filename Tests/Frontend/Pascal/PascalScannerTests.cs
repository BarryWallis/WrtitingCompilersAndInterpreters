using System.IO;
using Moq;
using Xunit;
using FrontendComponents;
using FrontendComponents.Pascal;

public class PascalScannerTests
{
    [Fact]
    public void ExtractToken_EOFCharacter_ReturnsEofToken()
    {
        // Arrange
        var mockSource = new Mock<Source>(new StreamReader(new MemoryStream()));
        mockSource.Setup(s => s.GetCurrentChar()).Returns(Source.EOF);
        var scanner = new PascalScanner(mockSource.Object);

        // Act
        var token = scanner.ExtractToken();

        // Assert
        Assert.IsType<EofToken>(token);
    }

    [Fact]
    public void ExtractToken_NonEOFCharacter_ReturnsPlaceholderToken()
    {
        // Arrange
        var mockSource = new Mock<Source>(new StreamReader(new MemoryStream()));
        mockSource.Setup(s => s.GetCurrentChar()).Returns('a');
        var scanner = new PascalScanner(mockSource.Object);

        // Act
        var token = scanner.ExtractToken();

        // Assert
        Assert.IsType<PlaceholderToken>(token);
    }
}