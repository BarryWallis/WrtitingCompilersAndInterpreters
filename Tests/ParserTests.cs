using FrontendLib;
using FrontendLib.Pascal;

using MessageLib;

using Xunit;

namespace Tests;

public class ParserTests
{
    private sealed class CollectingListener : IMessageListener
    {
        public List<Message> Messages { get; } = [];
        public void MessageReceived(Message message) => Messages.Add(message);
    }

    private static Source CreateSource(string text)
        => new(new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text))));

    [Fact]
    public void PascalParserTopDown_ShouldEmitSummaryMessage()
    {
        Source source = CreateSource("BEGIN END");
        PascalScanner scanner = new(source);
        PascalParserTopDown parser = new(scanner);
        CollectingListener listener = new();
        parser.AddMessageListener(listener);
        parser.Parse();
        Assert.Contains(listener.Messages, m => m is ParserSummaryMessage);
    }
}
