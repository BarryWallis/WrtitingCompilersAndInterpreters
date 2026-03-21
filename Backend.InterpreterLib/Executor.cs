using System.Diagnostics;

using IntermediateLib;
using MessageLib;

namespace Backend.InterpreterLib;

/// <summary>
/// Executes intermediate code and publishes interpreter status messages.
/// </summary>
public class Executor : BackendLib.Backend
{
    /// <summary>
    /// Processes intermediate code and publishes an interpreter summary message.
    /// </summary>
    /// <param name="intermediateCode">The intermediate code to process.</param>
    /// <param name="symbolTable">The symbol table associated with the intermediate code.</param>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTable symbolTable)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        int executionCount = 0;
        int runtimeErrors = 0;

        stopwatch.Stop();
        float elapsedTime = (float)stopwatch.Elapsed.TotalSeconds;

        SendMessage(new InterpreterSummaryMessage(executionCount, runtimeErrors, elapsedTime));
    }
}
