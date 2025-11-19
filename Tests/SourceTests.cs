using FrontendLib;

using MessageLib;

using Xunit;

namespace Tests;

public class SourceTests
{
    private sealed class CaptureListener : IMessageListener
    {
        public List<SourceLineMessage> Lines { get; } = [];
        public void MessageReceived(Message message)
        {
            if (message is SourceLineMessage slm)
            {
                Lines.Add(slm);
            }
        }
    }

    [Fact]
    public void CurrentCharacter_ShouldReturnEndOfFile_ForEmpty()
    {
        Source source = new(new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(string.Empty))));
        Assert.Equal(Source.EndOfFile, source.CurrentCharacter);
    }

    [Fact]
    public void GetNextCharacter_ShouldAdvance()
    {
        Source source = new(new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes("AB"))));
        char first = source.CurrentCharacter;
        char second = source.GetNextCharacter();
        Assert.NotEqual(first, second);
    }
}
