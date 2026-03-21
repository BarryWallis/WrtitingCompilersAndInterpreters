using Backend.InterpreterLib;
using IntermediateLib;
using MessageLib;

using Xunit;

namespace Backend.Interpreter.Tests;

public sealed class ExecutorTests
{
    /// <summary>
    /// Verifies <see cref="Executor.Process(IIntermediateCode, ISymbolTable)"/> publishes an <see cref="InterpreterSummaryMessage"/>.
    /// </summary>
    [Fact]
    public void ProcessPublishesInterpreterSummaryMessage()
    {
        Executor executor = new();
        RecordingMessageListener listener = new();
        IIntermediateCode intermediateCode = new TestIntermediateCode();
        ISymbolTable symbolTable = new TestSymbolTable();

        executor.AddMessageListener(listener);

        try
        {
            executor.Process(intermediateCode, symbolTable);
        }
        finally
        {
            executor.RemoveMessageListener(listener);
        }

        InterpreterSummaryMessage message = Assert.IsType<InterpreterSummaryMessage>(listener.LastMessage);
        Assert.Equal(0, message.ExecutionCount);
        Assert.Equal(0, message.RuntimeErrors);
    }

    /// <summary>
    /// Verifies <see cref="Executor.Process(IIntermediateCode, ISymbolTable)"/> reports a non-negative elapsed execution time.
    /// </summary>
    [Fact]
    public void ProcessPublishesNonNegativeElapsedTime()
    {
        Executor executor = new();
        RecordingMessageListener listener = new();
        IIntermediateCode intermediateCode = new TestIntermediateCode();
        ISymbolTable symbolTable = new TestSymbolTable();

        executor.AddMessageListener(listener);

        try
        {
            executor.Process(intermediateCode, symbolTable);
        }
        finally
        {
            executor.RemoveMessageListener(listener);
        }

        InterpreterSummaryMessage message = Assert.IsType<InterpreterSummaryMessage>(listener.LastMessage);
        Assert.True(message.ElapsedTime >= 0f);
    }

    /// <summary>
    /// Minimal test implementation of <see cref="IIntermediateCode"/>.
    /// </summary>
    private sealed class TestIntermediateCode : IIntermediateCode
    {
    }

    /// <summary>
    /// Minimal test implementation of <see cref="ISymbolTable"/>.
    /// </summary>
    private sealed class TestSymbolTable : ISymbolTable
    {
    }

    /// <summary>
    /// Records the most recently received message.
    /// </summary>
    private sealed class RecordingMessageListener : IMessageListener
    {
        /// <summary>
        /// Gets the most recently received message.
        /// </summary>
        public Message? LastMessage { get; private set; }

        /// <summary>
        /// Captures a received message.
        /// </summary>
        /// <param name="message">The received message.</param>
        public void MessageReceived(Message message) => LastMessage = message;
    }
}
