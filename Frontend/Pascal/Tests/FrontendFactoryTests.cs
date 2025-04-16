using System;
using Xunit;
using System.Diagnostics;

namespace FrontendComponents.Pascal.Tests;

public class FrontendFactoryTests
{
    [Fact]
    public void CreateParser_ValidPascalTopDown_ReturnsPascalParserTopDown()
    {
        // Arrange
        Source source = new(new StreamReader(new MemoryStream()));

        // Act
        Parser parser = FrontendFactory.CreateParser("pascal", "top-down", source);

        // Assert
        _ = Assert.IsType<PascalParserTopDown>(parser);
    }

    [Fact]
    public void CreateParser_UnsupportedLanguage_ThrowsUnsupportedLanguageException()
    {
        // Arrange
        Source source = new(new StreamReader(new MemoryStream()));

        // Act & Assert
        _ = Assert.Throws<UnsupportedLanguageException>(() =>
            FrontendFactory.CreateParser("java", "top-down", source));
    }

    [Fact]
    public void CreateParser_UnsupportedParserType_ThrowsUnsupportedParserTypeException()
    {
        // Arrange
        Source source = new(new StreamReader(new MemoryStream()));

        // Act & Assert
        _ = Assert.Throws<UnsupportedParserTypeException>(() =>
            FrontendFactory.CreateParser("pascal", "bottom-up", source));
    }
}