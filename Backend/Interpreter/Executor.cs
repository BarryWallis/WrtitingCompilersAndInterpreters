using IntermediateLib;

using MessageLib;

namespace BackendLib.Interpreter;

/// <summary>
/// Backend implementation that would interpret (execute) the intermediate code.
/// Currently a stub that reports execution metrics with zero executions and errors.
/// </summary>
public class Executor : Backend
{
    /// <summary>
    /// Executes the provided <see cref="IIntermediateCode"/>. This stub only records timing and emits
    /// an <see cref="InterpreterSummaryMessage"/> reporting zero executions and errors.
    /// </summary>
    /// <param name="intermediateCode">The intermediate code to execute.</param>
    /// <param name="symbolTable">The associated symbol table used for runtime lookup.</param>
    public override void Process(IIntermediateCode? intermediateCode, ISymbolTable? symbolTable)
    {
        DateTime startTime = DateTime.Now;
        // Real implementation would traverse the intermediateCode and perform computations.
        TimeSpan elapsedTime = DateTime.Now - startTime;
        int executionCount = 0;
        int runtimeErrors = 0;

        SendMessage(new InterpreterSummaryMessage(executionCount, runtimeErrors, elapsedTime));
    }
}
