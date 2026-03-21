using Backend.CompilerLib;
using IntermediateLib;
using MessageLib;

using Xunit;

namespace Backend.Compiler.Tests;

public sealed class CodeGeneratorTests
{
    /// <summary>
    /// Verifies Process publishes a <see cref="CompilerSummaryMessage"/>.
    /// </summary>
    [Fact]
    public void ProcessPublishesCompilerSummaryMessage()
    {
        Backend.CompilerLib.CodeGenerator codeGenerator = new();
        RecordingMessageListener listener = new();
        IIntermediateCode intermediateCode = new TestIntermediateCode();
        ISymbolTable symbolTable = new TestSymbolTable();

        codeGenerator.AddMessageListener(listener);

        try
        {
            codeGenerator.Process(intermediateCode, symbolTable);
        }
        finally
        {
            codeGenerator.RemoveMessageListener(listener);
        }

        CompilerSummaryMessage message = Assert.IsType<CompilerSummaryMessage>(listener.LastMessage);
        Assert.Equal(0, message.InstructionCount);
    }

    /// <summary>
    /// Verifies Process reports a non-negative elapsed code generation time.
    /// </summary>
    [Fact]
    public void ProcessPublishesNonNegativeElapsedTime()
    {
        CodeGenerator codeGenerator = new();
        RecordingMessageListener listener = new();
        IIntermediateCode intermediateCode = new TestIntermediateCode();
        ISymbolTable symbolTable = new TestSymbolTable();

        codeGenerator.AddMessageListener(listener);

        try
        {
            codeGenerator.Process(intermediateCode, symbolTable);
        }
        finally
        {
            codeGenerator.RemoveMessageListener(listener);
        }

        CompilerSummaryMessage message = Assert.IsType<CompilerSummaryMessage>(listener.LastMessage);
        Assert.True(message.ElapsedCodeGenerationTime >= 0f);
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
