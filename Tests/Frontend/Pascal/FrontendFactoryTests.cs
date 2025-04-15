using System;
using Xunit;
using FrontendComponents.Pascal;

public class FrontendFactoryTests
{
    [Fact]
    public void CreateParser_ValidPascalTopDown_ReturnsPascalParserTopDown()
    {
        // Arrange
        var source = new Source(new StreamReader(new MemoryStream()));

        // Act
        var parser = FrontendFactory.CreateParser("pascal", "top-down", source);

        // Assert
        Assert.IsType<PascalParserTopDown>(parser);
    }

    [Fact]
    public void CreateParser_UnsupportedLanguage_ThrowsUnsupportedLanguageException()
    {
        // Arrange
        var source = new Source(new StreamReader(new MemoryStream()));

        // Act & Assert
        Assert.Throws<UnsupportedLanguageException>(() =>
            FrontendFactory.CreateParser("java", "top-down", source));
    }

    [Fact]
    public void CreateParser_UnsupportedParserType_ThrowsUnsupportedParserTypeException()
    {
        // Arrange
        var source = new Source(new StreamReader(new MemoryStream()));

        // Act & Assert
        Assert.Throws<UnsupportedParserTypeException>(() =>
            FrontendFactory.CreateParser("pascal", "bottom-up", source));
    }

    [Fact]
    public void CreateParser_UnreachableCode_ThrowsUnreachableException()
    {
        // Arrange
        var source = new Source(new StreamReader(new MemoryStream()));

        // Act & Assert
        Assert.Throws<UnreachableException>(() =>
            FrontendFactory.CreateParser("pascal", "unknown", source));
    }
}