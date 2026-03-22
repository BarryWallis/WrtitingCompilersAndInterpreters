using System;
using System.IO;
using System.Reflection;

using Frontend.PascalLib;

using MessageLib;

using Tokenizer;

using Xunit;

namespace Frontend.Tests;

public sealed class TokenizerListenerTests
{
    /// <summary>
    /// Verifies the source listener writes the formatted source line.
    /// </summary>
    [Fact]
    public void SourceMessageListenerWritesFormattedSourceLine()
    {
        IMessageListener listener = CreateListener("SourceMessageListener");
        SourceLineMessage message = new(7, "begin end.");

        string output = CaptureConsoleOut(() => listener.MessageReceived(message));

        Assert.Equal("007 begin end.\n", NormalizeNewLines(output));
    }

    /// <summary>
    /// Verifies the parser listener writes token details and quotes string values.
    /// </summary>
    [Fact]
    public void ParserMessageListenerWritesFormattedTokenAndQuotedStringValue()
    {
        IMessageListener listener = CreateListener("ParserMessageListener");
        TokenMessage message = new(12, 3, typeof(string), "'hello'", "hello");

        string output = CaptureConsoleOut(() => listener.MessageReceived(message));

        Assert.Equal(
            ">>> String          line=012, pos= 3, text=\"'hello'\"\n>>>                 value=\"hello\"\n",
            NormalizeNewLines(output));
    }

    /// <summary>
    /// Verifies the parser listener writes formatted syntax error details for syntax-error-shaped messages.
    /// </summary>
    [Fact]
    public void ParserMessageListenerWritesFormattedSyntaxError()
    {
        IMessageListener listener = CreateListener("ParserMessageListener");
        SyntaxErrorMessage message = new(4, 3, "bad", "unexpected token");

        string output = CaptureConsoleOut(() => listener.MessageReceived(message));

        Assert.Equal("       ^\n*** unexpected token [at \"bad\"]\n", NormalizeNewLines(output));
    }

    /// <summary>
    /// Verifies the parser listener ignores messages that are neither token nor syntax error messages.
    /// </summary>
    [Fact]
    public void ParserMessageListenerIgnoresParserSummaryMessage()
    {
        IMessageListener listener = CreateListener("ParserMessageListener");
        ParserSummaryMessage message = new(10, 0, 0.25f);

        string output = CaptureConsoleOut(() => listener.MessageReceived(message));

        Assert.Equal(string.Empty, output);
    }

    /// <summary>
    /// Verifies the backend listener writes the compiler summary.
    /// </summary>
    [Fact]
    public void BackendMessageListenerWritesCompilerSummary()
    {
        IMessageListener listener = CreateListener("BackendMessageListener");
        CompilerSummaryMessage message = new(42, 1.5f);

        string output = CaptureConsoleOut(() => listener.MessageReceived(message));

        Assert.Equal(
            "\n                  42 instructions generated.\n                1.50 seconds total code generation time.\n",
            NormalizeNewLines(output));
    }

    /// <summary>
    /// Verifies the backend listener writes the interpreter summary.
    /// </summary>
    [Fact]
    public void BackendMessageListenerWritesInterpreterSummary()
    {
        IMessageListener listener = CreateListener("BackendMessageListener");
        InterpreterSummaryMessage message = new(8, 1, 0.5f);

        string output = CaptureConsoleOut(() => listener.MessageReceived(message));

        Assert.Equal(
            "\n                   8 statements executed.\n                   1 runtime errors.\n                0.50 seconds total execution time.\n",
            NormalizeNewLines(output));
    }

    /// <summary>
    /// Creates a Tokenizer message listener instance through reflection.
    /// </summary>
    /// <param name="nestedTypeName">The private nested listener type name.</param>
    /// <returns>The instantiated listener.</returns>
    private static IMessageListener CreateListener(string nestedTypeName)
    {
        Type? listenerType = typeof(Pascal).GetNestedType(nestedTypeName, BindingFlags.NonPublic);
        Assert.NotNull(listenerType);

        object? instance = Activator.CreateInstance(listenerType, nonPublic: true);
        IMessageListener listener = Assert.IsAssignableFrom<IMessageListener>(instance);
        return listener;
    }

    /// <summary>
    /// Captures console output produced by the provided action.
    /// </summary>
    /// <param name="action">The action that writes to the console.</param>
    /// <returns>The captured output.</returns>
    private static string CaptureConsoleOut(Action action)
    {
        TextWriter originalWriter = Console.Out;
        StringWriter writer = new();

        try
        {
            Console.SetOut(writer);
            action();
            return writer.ToString();
        }
        finally
        {
            Console.SetOut(originalWriter);
        }
    }

    /// <summary>
    /// Normalizes line endings for stable assertions.
    /// </summary>
    /// <param name="text">The text to normalize.</param>
    /// <returns>The text with normalized line endings.</returns>
    private static string NormalizeNewLines(string text) => text.Replace("\r\n", "\n", StringComparison.Ordinal);

    /// <summary>
    /// Test message whose runtime shape matches the parser listener's syntax error expectations.
    /// </summary>
    /// <param name="LineNumber">The source line number.</param>
    /// <param name="Position">The source position.</param>
    /// <param name="TokenText">The offending token text.</param>
    /// <param name="ErrorMessage">The syntax error message.</param>
    private sealed record SyntaxErrorMessage(int LineNumber, int Position, string? TokenText, string ErrorMessage) : Message;
}
