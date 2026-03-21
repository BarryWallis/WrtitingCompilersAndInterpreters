using System;
using System.IO;
using System.Text;

using Frontend.CompositionLib;
using Frontend.PascalLib;
using FrontendLib;

using Xunit;

namespace Frontend.Tests;

public sealed class FrontendFactoryTests
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
    /// Verifies the factory creates a Pascal top-down parser for matching language and type.
    /// </summary>
    [Fact]
    public void CreateParserWhenPascalTopDownReturnsPascalParserTopDown()
    {
        StreamReader reader = CreateReader("program");
        Source source = new(reader);

        Parser parser = FrontendFactory.CreateParser("Pascal", "top-down", source);

        _ = Assert.IsType<PascalParserTopDown>(parser);
    }

    /// <summary>
    /// Verifies parser creation is case-insensitive for language and type names.
    /// </summary>
    [Fact]
    public void CreateParserWhenLanguageAndTypeHaveDifferentCaseReturnsPascalParserTopDown()
    {
        StreamReader reader = CreateReader("program");
        Source source = new(reader);

        Parser parser = FrontendFactory.CreateParser("pAsCaL", "TOP-DOWN", source);

        _ = Assert.IsType<PascalParserTopDown>(parser);
    }

    /// <summary>
    /// Verifies the factory throws when language is unsupported.
    /// </summary>
    [Fact]
    public void CreateParserWhenLanguageIsInvalidThrowsException()
    {
        StreamReader reader = CreateReader("program");
        Source source = new(reader);

        Exception exception = Assert.Throws<Exception>(() => FrontendFactory.CreateParser("Java", "top-down", source));

        Assert.Equal("Parser factory: Invalid language 'Java'", exception.Message);
    }

    /// <summary>
    /// Verifies the factory throws when parser type is unsupported for Pascal.
    /// </summary>
    [Fact]
    public void CreateParserWhenTypeIsInvalidThrowsException()
    {
        StreamReader reader = CreateReader("program");
        Source source = new(reader);

        Exception exception = Assert.Throws<Exception>(() => FrontendFactory.CreateParser("Pascal", "bottom-up", source));

        Assert.Equal("Parser factory: Invalid type 'bottom-up'", exception.Message);
    }
}
