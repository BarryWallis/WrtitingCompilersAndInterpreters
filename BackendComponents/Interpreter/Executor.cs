using System.Diagnostics;

using Intermediate;
using Intermediate.Implementation;

using Messages;

namespace BackendComponents.Interpreter;

/// <summary>
/// Executes intermediate code while tracking execution time, error count, and sending a summary message. 
/// </summary>
public class Executor : Backend
{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2211 // Non-constant fields should not be visible
    protected static int _executionCount = 0;
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning restore IDE0079 // Remove unnecessary suppression

    /// <summary>
    /// Initializes a new instance of the <see cref="Executor"/> class.
    /// </summary>
    public Executor()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Executor"/> class using the specified parent executor.
    /// </summary>
    /// <param name="parent">The parent executor to inherit from.</param>
#pragma warning disable IDE0060 // Remove unused parameter
    public Executor(Executor parent) : base()
#pragma warning restore IDE0060 // Remove unused parameter
    {
    }

    /// <summary>
    /// Processes intermediate code and symbol table, measuring execution time and counting errors.
    /// </summary>
    /// <param name="intermediateCode">Represents the code to be processed during execution.</param>
    /// <param name="symbolTableStack">
    /// Contains the symbols and their associated information used in the code.
    /// </param>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTableStack symbolTableStack)
    {
        SymbolTableStack = symbolTableStack;
        IntermediateCode = intermediateCode;

        Stopwatch stopwatch = Stopwatch.StartNew();

        Debug.Assert(intermediateCode.Root is not null);
        IIntermediateCodeNode rootNode = intermediateCode.Root;
        StatementExecutor statementExecutor = new(this);
        _ = statementExecutor.Execute(rootNode);

        stopwatch.Stop();
        double elapsedTime = stopwatch.Elapsed.TotalSeconds;
        int runtimeErrors = RuntimeErrorHandler.ErrorCount;

        SendMessage(new InterpreterSummaryMessage(_executionCount, runtimeErrors, elapsedTime));
    }
}
