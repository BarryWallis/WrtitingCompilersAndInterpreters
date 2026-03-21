using System.Diagnostics;

using IntermediateLib;
using MessageLib;

namespace Backend.CompilerLib;

/// <summary>
/// Generates backend output from intermediate code and reports generation metrics.
/// </summary>
public class CodeGenerator : BackendLib.Backend
{
    /// <summary>
    /// Processes the provided intermediate code and symbol table, then publishes a compiler summary message.
    /// </summary>
    /// <param name="intermediateCode">The intermediate representation to process.</param>
    /// <param name="symbolTable">The symbol table associated with the intermediate representation.</param>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTable symbolTable)
    {
        long startTimestamp = Stopwatch.GetTimestamp();
        float elapsedTime = (float)Stopwatch.GetElapsedTime(startTimestamp).TotalSeconds;
        int instructionCount = 0;

        SendMessage(new CompilerSummaryMessage(instructionCount, elapsedTime));
    }
}
