using BackendLib.Compiler;
using BackendLib.Interpreter;

using IntermediateLib;

using MessageLib;

using Xunit;

namespace Tests;

public class BackendSummaryMessageTests
{
    private sealed class CollectingListener : IMessageListener
    {
        public List<Message> Messages { get; } = [];
        public void MessageReceived(Message message) => Messages.Add(message);
    }

    private sealed class DummyIntermediate : IIntermediateCode { }
    private sealed class DummySymbols : ISymbolTable { }

    [Fact]
    public void CodeGenerator_ShouldEmitCompilerSummaryMessage()
    {
        CollectingListener listener = new();
        BackendLib.Backend.MessageHandler.AddListener(listener);
        try
        {
            CodeGenerator gen = new();
            gen.Process(new DummyIntermediate(), new DummySymbols());
            Assert.Contains(listener.Messages, m => m is CompilerSummaryMessage);
        }
        finally
        {
            BackendLib.Backend.MessageHandler.RemoveListener(listener);
        }
    }

    [Fact]
    public void Executor_ShouldEmitInterpreterSummaryMessage()
    {
        CollectingListener listener = new();
        BackendLib.Backend.MessageHandler.AddListener(listener);
        try
        {
            Executor exe = new();
            exe.Process(new DummyIntermediate(), new DummySymbols());
            Assert.Contains(listener.Messages, m => m is InterpreterSummaryMessage);
        }
        finally
        {
            BackendLib.Backend.MessageHandler.RemoveListener(listener);
        }
    }
}
