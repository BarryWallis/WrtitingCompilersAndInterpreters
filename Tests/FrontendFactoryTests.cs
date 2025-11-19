using FrontendLib;

using Xunit;

namespace Tests;

public class FrontendFactoryTests
{
    private static Source CreateSource(string text)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
        return new Source(new StreamReader(new MemoryStream(bytes)));
    }

    [Fact]
    public void CreateParser_PascalTopDown()
    {
        Parser parser = FrontendFactory.CreateParser("Pascal", "top-down", CreateSource(""));
        Assert.NotNull(parser);
    }

    [Fact]
    public void CreateParser_InvalidLanguage_ShouldThrow() => Assert.Throws<ArgumentException>(() => FrontendFactory.CreateParser("Other", "top-down", CreateSource("")));

    [Fact]
    public void CreateParser_InvalidType_ShouldThrow() => Assert.Throws<ArgumentException>(() => FrontendFactory.CreateParser("Pascal", "Other", CreateSource("")));
}
